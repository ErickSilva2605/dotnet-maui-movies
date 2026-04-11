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

## Estratégia Offline-First

O fluxo offline-first é responsabilidade do **Use Case** — não do Repository nem da ViewModel.

### Fluxo padrão

```
1. Busca dados locais no SQLite
2. Exibe imediatamente (pode ser dados desatualizados)
3. Busca dados na API em paralelo/sequencial
4. Salva dados novos no SQLite
5. Retorna dados atualizados
6. UI reage automaticamente via ObservableCollection
```

### Implementação no Use Case

```csharp
public class GetTrendingUseCase
{
    public async Task<List<Movie>> ExecuteAsync()
    {
        // 1. retorna cache local imediatamente
        var local = await _localRepo.GetTrendingAsync();

        // 2. busca remoto
        var remote = await _remoteSource.GetTrendingAsync();

        if (remote.IsSuccess)
        {
            // 3. atualiza cache
            await _localRepo.SaveTrendingAsync(remote.Value);
            return remote.Value;
        }

        // 4. sem conexão: retorna o que tem local
        return local;
    }
}
```

### Dados parciais vs completos

| Contexto | Dados disponíveis | Comportamento |
|---|---|---|
| Lista (Home, Search) | Parciais (título, poster, rating) | Exibe imediatamente do cache |
| Tela de detalhe | Completos (elenco, trailers, etc) | Busca completo se necessário |
| Primeira abertura sem internet | Vazio | Exibe empty state |

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
