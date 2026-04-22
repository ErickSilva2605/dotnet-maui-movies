# Fluxo de Dados

## As 4 Representações

O mesmo dado percorre 4 formas distintas ao longo das camadas. Cada representação tem uma responsabilidade única e não vaza para outras camadas.

| Representação | Camada | Responsabilidade | Dependência |
|---|---|---|---|
| `MovieDto` | Infrastructure/Api | Contrato da API TMDB | `System.Text.Json` |
| `MovieEntity` | Infrastructure/Persistence | Contrato do SQLite | EF Core |
| `Movie` | Core/Entities | Linguagem do domínio | Nenhuma |
| `MovieModel` | Core/Models | Estado observável para UI | `CommunityToolkit.Mvvm` |

---

## Por que 4 representações?

Cada uma existe porque as outras **não conseguem fazer o seu trabalho**:

- `MovieDto` — tem `poster_path`, `vote_average`, campos snake_case, campos que não interessam ao domínio
- `MovieEntity` — não pode ter `List<Genre>` (SQLite é relacional), tem configurações EF Core
- `Movie` — POCO puro, sem atributos de framework. É o contrato neutro que permite Infrastructure e Core se comunicarem sem acoplamento
- `MovieModel` — ObservableObject com estado de UI (`IsFavorite`, `IsSelected`). Use Cases não devem instanciar objetos observáveis

---

## Fluxo via API

```
TMDB API
    ↓
MovieDto                   Infrastructure/Api/Dtos
    ↓  TmdbMovieDataSource (mapper)
Movie                      Core/Entities
    ↓  UseCase retorna
Movie                      Core/UseCases
    ↓  ViewModel transforma
MovieModel                 Core/Models
    ↓  binding
Page.xaml                  UI/Pages
```

## Fluxo via SQLite

```
SQLite
    ↓
MovieEntity                Infrastructure/Persistence/Entities
    ↓  Repository (mapper)
Movie                      Core/Entities
    ↓  UseCase retorna
Movie                      Core/UseCases
    ↓  ViewModel transforma
MovieModel                 Core/Models
    ↓  binding
Page.xaml                  UI/Pages
```

---

## Mapeamentos

### DTO → Domain Entity
Feito na camada Infrastructure, dentro do DataSource ou em um mapper dedicado.

```
Infrastructure/Api/Mapping/MovieDtoMapper.cs
```

### SQLite Entity → Domain Entity
Feito na camada Infrastructure, dentro do Repository ou em um mapper dedicado.

```
Infrastructure/Persistence/Mapping/MovieEntityMapper.cs
```

### Domain Entity → Observable Model
Feito na camada Core, dentro da ViewModel (ou factory de Model).

```
Core/ViewModels/HomeViewModel.cs
  movies.Select(m => new MovieModel(m))
```

---

## Estratégia de Cache — Network-First com Fallback Offline

A responsabilidade da estratégia de cache é do **Use Case** — não do Repository nem da ViewModel.

### Decisão arquitetural

A estratégia adotada é **network-first com fallback offline**, não offline-first puro:

- **Online:** sempre busca da API → salva no SQLite → exibe (dados sempre frescos)
- **Offline:** lê do SQLite → exibe com indicador visual de "dados offline"
- **Sem TTL:** a distinção online/offline substitui qualquer lógica de expiração — quando há internet, os dados são sempre os da API

Essa decisão foi tomada conscientemente: o app é de discovery de conteúdo (filmes, séries, pessoas). Dado desatualizado tem impacto negligenciável, mas dado incorreto por TTL mal calibrado seria ruído desnecessário.

### Fluxo de leitura

```
UseCase.ExecuteAsync()
    ↓
Tem internet?
    ├── SIM → busca na API → salva no SQLite → retorna
    └── NÃO → lê do SQLite → retorna (com flag IsOffline = true)
```

### Implementação no Use Case

```csharp
public class GetTrendingMoviesUseCase
{
    public async Task<List<Movie>> ExecuteAsync()
    {
        if (_connectivity.IsConnected)
        {
            var movies = await _remoteSource.GetTrendingAsync();
            await _repository.SaveTrendingAsync(movies);
            return movies;
        }

        return await _repository.GetTrendingAsync();
    }
}
```

### Paginação e cache orgânico

O cache cresce naturalmente conforme o usuário navega — não há prefetch agressivo:

- Página 1 é salva no primeiro load
- Páginas seguintes são salvas conforme o usuário scrolla
- Offline exibe tudo que já foi carregado, ordenado por posição
- Ao chegar no fim do cache offline, exibe mensagem: "Conecte-se para ver mais"

### Schema SQLite — campo de posição

```csharp
public class MovieEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string PosterPath { get; set; }
    // ... demais campos
    public string ListType { get; set; }   // "trending", "popular", "top_rated"
    public int Position { get; set; }      // ordem global na lista
    public DateTime CachedAt { get; set; } // auditoria, não usado pra TTL
}
```

### Ações do usuário (favoritos, watchlist, avaliações)

Requerem internet. Sem fila de sync offline, sem write-ahead queue.  
Se o usuário tentar favoritar sem conexão, exibe mensagem e bloqueia a ação.

### Dados parciais vs completos

| Contexto | Dados disponíveis | Comportamento |
|---|---|---|
| Lista (Home, Popular...) | Parciais (título, poster, rating) | Cache orgânico por posição |
| Tela de detalhe | Completos (elenco, trailers, etc) | Salvo quando o usuário abre |
| Busca | Nenhum | Não cacheada — requer internet |
| Primeira abertura sem internet | Vazio | Empty state |

### Imagens

Cache em disco via **FFImageLoading.Maui**. Independente do SQLite — a biblioteca gerencia automaticamente. Skeleton exibido na primeira carga, zero loading nas subsequentes.

---

## Observable Model — separação de estado

`Movie` (domain entity) carrega dados do domínio. `MovieModel` adiciona estado de UI reativo:

```csharp
// Core/Entities/Movie.cs — imutável, sem framework
public class Movie
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string PosterPath { get; init; }
    public double VoteAverage { get; init; }
    public IReadOnlyList<Genre> Genres { get; init; }
}

// Core/Models/MovieModel.cs — observável, estado de UI
public partial class MovieModel : ObservableObject
{
    // dados do domínio (imutáveis)
    public int Id { get; }
    public string Title { get; }
    public string PosterPath { get; }
    public double VoteAverage { get; }

    // estado de UI (reativo)
    [ObservableProperty] bool _isFavorite;
    [ObservableProperty] bool _isInWatchlist;
    [ObservableProperty] bool _isSelected;

    public MovieModel(Movie movie)
    {
        Id = movie.Id;
        Title = movie.Title;
        PosterPath = movie.PosterPath;
        VoteAverage = movie.VoteAverage;
    }
}
```
