# Arquitetura

## Visão Geral

O projeto segue **Clean Architecture** combinada com **MVVM**, organizada em 3 camadas (projetos):

```
MauiMovies.Core           → domínio, lógica, ViewModels
MauiMovies.Infrastructure → implementações de API e banco de dados
MauiMovies.UI             → interface MAUI, pages, serviços de plataforma
```

### Direção das dependências

```
UI  →  Core  ←  Infrastructure
```

- `Core` não depende de ninguém
- `Infrastructure` depende de `Core`
- `UI` depende de `Core`
- `Infrastructure` e `UI` nunca se conhecem diretamente

---

## Estrutura de Projetos

```
src/
├── MauiMovies.Core/
│   ├── Entities/               → entidades de domínio (POCOs puros)
│   ├── Models/                 → modelos observáveis para UI (ObservableObject)
│   ├── Interfaces/
│   │   ├── Repositories/       → contratos de acesso local (IMovieRepository...)
│   │   ├── DataSources/        → contratos de acesso remoto (IMovieRemoteDataSource...)
│   │   └── Services/           → contratos de serviços de plataforma (INavigationService, IDialogService...)
│   ├── UseCases/
│   │   ├── Trending/           → GetTrendingUseCase
│   │   ├── Movies/             → GetMovieDetailsUseCase, SearchMoviesUseCase...
│   │   ├── Tv/                 → GetTvDetailsUseCase...
│   │   └── Favorites/          → ToggleFavoriteUseCase, GetFavoritesUseCase...
│   └── ViewModels/             → ViewModels por tela (sem dependência de MAUI)
│
├── MauiMovies.Infrastructure/
│   ├── Api/
│   │   ├── Http/               → IRequestProvider, RequestProvider
│   │   ├── DataSources/        → TmdbMovieDataSource, TmdbTrendingDataSource...
│   │   ├── Dtos/               → objetos de transferência da API TMDB
│   │   ├── Converters/         → conversores JSON (ex: PolymorphicListConverter)
│   │   └── Mapping/            → DTO → Domain Entity
│   ├── Persistence/
│   │   ├── AppDbContext.cs
│   │   ├── DatabaseInitializer.cs
│   │   ├── Entities/           → entidades mapeadas para SQLite
│   │   ├── Configurations/     → Fluent API (EF Core)
│   │   ├── Repositories/       → implementações de IMovieRepository...
│   │   ├── Mapping/            → SQLite Entity → Domain Entity
│   │   └── Migrations/         → migrations do EF Core
│   └── DI/                     → InfrastructureExtensions (registro de DI)
│
└── MauiMovies.UI/
    ├── Pages/                  → XAML pages (code-behind mínimo)
    ├── Controls/               → componentes reutilizáveis
    ├── Services/               → MauiNavigationService, MauiDialogService...
    ├── Resources/              → estilos, cores, fontes, imagens
    ├── Navigation/             → Routes.cs (constantes de rota)
    ├── Constants/              → ResourceKeys.cs (chaves de ResourceDictionary)
    └── DI/                     → UIExtensions (registro de DI + path do SQLite)

tests/
├── MauiMovies.Core.Tests/           → xUnit + Moq
├── MauiMovies.Infrastructure.Tests/ → xUnit + SQLite in-memory + HTTP mock
└── MauiMovies.UITests.*/            → Appium (Android, iOS, macOS, Windows)
```

---

## Responsabilidades por Camada

### Core

Camada central da aplicação. Não referencia nenhum framework externo além do `CommunityToolkit.Mvvm`.

| Pasta | Responsabilidade |
|---|---|
| `Entities/` | Representação pura do domínio. Sem atributos de banco ou serialização |
| `Models/` | Wrappers observáveis das entities para binding na UI |
| `Interfaces/` | Contratos que definem o que Infrastructure e UI devem implementar |
| `UseCases/` | Orquestração de uma única operação (offline-first, regras de negócio) |
| `ViewModels/` | Estado e comandos de cada tela. Sem referência a MAUI |

### Infrastructure

Implementações concretas de acesso a dados. Conhece `Core`, não conhece `UI`.

| Pasta | Responsabilidade |
|---|---|
| `Api/` | Comunicação HTTP com a API TMDB |
| `Persistence/` | Leitura e escrita no banco SQLite via EF Core |
| `DI/` | Registro de todas as dependências da camada |

### UI

Projeto MAUI. Responsável exclusivamente por renderização e interação com o usuário.

| Pasta | Responsabilidade |
|---|---|
| `Pages/` | XAML com binding nas ViewModels do Core |
| `Controls/` | Componentes visuais reutilizáveis |
| `Services/` | Implementações MAUI de `INavigationService`, `IDialogService` etc |
| `DI/` | Registro das dependências de UI + configuração do SQLite path |

---

## Decisões Arquiteturais

### ViewModels no Core, não na UI

**Motivação:** o projeto UI é um projeto MAUI — requer inicialização de plataforma para rodar. Se ViewModels morassem na UI, testes xUnit precisariam de device ou emulador.

**Solução:** ViewModels em `Core` (`.NET Class Library` puro). Dependências de plataforma (navegação, dialogs, toasts) são abstraídas em interfaces definidas no próprio Core e implementadas na UI.

```
Core/Interfaces/Services/INavigationService.cs  → contrato
UI/Services/MauiNavigationService.cs             → implementação MAUI
```

### Use Cases, não Services

**Motivação:** Services agrupados por domínio crescem indefinidamente, acumulam dependências e dificultam testes isolados. Em desenvolvimento assistido por IA, esse problema se amplifica.

**Solução:** cada operação relevante é uma classe independente com um único método `ExecuteAsync`. Dependências são mínimas e explícitas. Registro via assembly scanning.

**Exceção:** operações triviais de leitura sem lógica podem ser agrupadas em um service simples.

### EF Core + SQLite para persistência

**Motivação:** Migrations automáticas, Fluent API para configuração de entidades, LINQ completo e showcase de ORM enterprise.

**Detalhe importante:** usar `IDbContextFactory<AppDbContext>` ao invés de `AddDbContext` — MAUI não tem escopo de request e o factory garante segurança em operações concorrentes.

### Estratégia de Cache

Network-first com fallback offline. Ver [02-data-flow.md](02-data-flow.md) para detalhes do fluxo.

### Retorno dos Use Cases — Result\<T\>

Use Cases retornam `Result<T>` próprio (implementação simples, sem biblioteca externa).

- **Infrastructure** deixa exceções propagarem naturalmente (HttpClient, EF Core)
- **Use Case** captura e converte em `Result.Fail(mensagem)`
- **ViewModel** recebe `Result<T>` e decide o que exibir — nunca acessa `.Value` sem verificar `.IsSuccess`

```csharp
// Core/Common/Result.cs
public class Result<T>
{
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(T value) { IsSuccess = true; Value = value; }
    private Result(string error) { IsSuccess = false; Error = error; }

    public static Result<T> Ok(T value) => new(value);
    public static Result<T> Fail(string error) => new(error);
}
```

### Navegação — Shell nativo + tab bar customizada sobreposta

**Motivação:** Shell nativo não permite customização visual da tab bar (cores, animações, formatos). Bibliotecas de terceiros como `SimpleToolkit.SimpleShell` resolvem isso, mas introduzem dependência de maintainer único. Criar as páginas como `ContentView` perderia lifecycle e DI nas ViewModels.

**Solução:** Shell nativo para roteamento + tab bar `ContentView` customizada injetada como overlay nativo de plataforma.

```
AppShell.xaml (Shell nativo)
└── ShellContent × 5   → ContentPages reais — lifecycle completo + ViewModels via DI

CustomTabBarView        → injetada via TabBarInjector como view nativa (Android FrameLayout / iOS UIWindow)
                          fora da hierarquia do Shell — zero flicker durante navegação entre abas
```

**Troca de aba:** `CurrentItem.CurrentItem = section` via `FindShellSectionByRoute` — equivalente ao que o Shell nativo faz internamente. Sem `GoToAsync`, sem animação de push, sem entrada no histórico.

**Tab bar nativa ocultada:** `Shell.TabBarIsVisible="False"` em cada `ContentPage`.

**Tab bar customizada — overlay de plataforma:** `TabBarInjector` (filename-based multi-targeting) injeta a view no nível de Activity/UIWindow, fora da hierarquia do Shell. Resultado: a tab bar nunca é recriada durante navegação.

```
Controls/Navigation/TabBarInjector.cs          → partial class — declara o contrato
Controls/Navigation/TabBarInjector.Android.cs  → Android: FrameLayout + GravityFlags.Bottom
Controls/Navigation/TabBarInjector.MaciOS.cs   → iOS/macOS: UIWindow subview com AutoresizingMask
Controls/Navigation/TabBarInjector.Windows.cs  → Windows: no-op
```

**Animações:** implementadas em `CustomTabBarView` usando `IDrawable` + `GraphicsView` + API de animação do MAUI (`Animation`, `Easing`). Referência de implementação: [RadekVyM/Navbar-Animation-1](https://github.com/RadekVyM/Navbar-Animation-1) — bolha circular com ícone elevado.

#### Hide on scroll (comportamento M3)

A tab bar se esconde ao rolar para cima e reaparece ao rolar para baixo — padrão Material Design 3.

**Telas que aplicam:** Home, Filmes, Séries, Pessoas (listas longas). Telas de detalhe não têm tab bar — são páginas empilhadas na stack.

**Implementação:** comunicação via `WeakReferenceMessenger` (CommunityToolkit.Mvvm — já dependência do projeto):

```
CollectionView.Scrolled → Page detecta direção do scroll
    → WeakReferenceMessenger.Send(new TabBarScrollMessage(hide: true/false))
        → AppShell recebe → anima TranslationY da CustomTabBarView
```

```csharp
await customTabBar.TranslateTo(0, customTabBar.Height, 250, Easing.CubicIn); // esconde
await customTabBar.TranslateTo(0, 0, 250, Easing.CubicOut);                  // mostra
```

### Navegação para páginas fora da hierarquia visual

Páginas de detalhe, perfil de usuário, configurações e demais fluxos secundários **não** fazem parte da hierarquia visual do Shell. São registradas programaticamente e navegadas via `GoToAsync`.

**Rotas:**

```csharp
// UI/Navigation/Routes.cs
static class Routes
{
    // abas (hierarquia visual — auto-registradas pelo Shell)
    public const string Home   = "home";
    public const string Movies = "movies";
    // ...

    // detalhes e fluxos secundários (registradas via Routing.RegisterRoute)
    public const string MovieDetail  = "movie-detail";
    public const string PersonDetail = "person-detail";
    public const string Settings     = "settings";
    // ...
}
```

**Registro no startup (UI):**

```csharp
Routing.RegisterRoute(Routes.MovieDetail, typeof(MovieDetailPage));
Routing.RegisterRoute(Routes.PersonDetail, typeof(PersonDetailPage));
```

**ViewModels não conhecem Shell nem Routes** — a navegação é abstraída em `INavigationService` com métodos tipados:

```csharp
// Core/Interfaces/Services/INavigationService.cs
public interface INavigationService
{
    Task NavigateToMovieDetailAsync(int movieId);
    Task NavigateToPersonDetailAsync(int personId);
    Task GoBackAsync();
}

// UI/Services/MauiNavigationService.cs
public class MauiNavigationService : INavigationService
{
    public async Task NavigateToMovieDetailAsync(int movieId) =>
        await Shell.Current.GoToAsync($"{Routes.MovieDetail}?id={movieId}");
}
```

**Por que métodos tipados e não `GoToAsync(string route)` na interface?** Se a interface expusesse uma string de rota, o ViewModel precisaria saber qual string passar — ou importa de `UI` (proibido pela regra de dependências) ou tem magic strings no `Core`. Com métodos tipados, a rota fica encapsulada em `MauiNavigationService`. O `Core` conhece apenas a intenção de navegação, nunca a rota concreta.

### Autenticação TMDB

**Duas camadas independentes:**

| Camada | Finalidade | Armazenamento |
|---|---|---|
| API Key | Acesso a todo conteúdo público (listas, detalhes, busca) | `appsettings.json` local (gitignored), injetado via DI |
| Session ID | Ações do usuário autenticado (favoritos, watchlist, avaliações) | `SecureStorage` do MAUI |

- API v3 (query string `?api_key=xxx` para chamadas públicas)
- Fluxo de login: Request Token → aprovação no site TMDB → Session ID
- Auth é **fase 2** — implementada quando as telas que dependem dela forem construídas
- Todo conteúdo de leitura funciona sem autenticação do usuário

---

## Constantes de UI — `ResourceKeys` e `Routes`

### ResourceKeys (`UI/Constants/ResourceKeys.cs`)

Centraliza todas as chaves do `ResourceDictionary` como constantes tipadas. Elimina magic strings em code-behind, converters e behaviors — falhas de digitação viram erro de compilação em vez de falha silenciosa em runtime.

**Quando usar:** qualquer código C# que precise resolver um recurso via `Application.Current.Resources` ou `AppThemeBinding` dinâmico — code-behind, converters, behaviors, handlers de plataforma.

```csharp
// ✅ Tipado — erro de compilação se a chave não existir em ResourceKeys
barColor = ResolveColor(isDark ? ResourceKeys.SurfaceDark : ResourceKeys.SurfaceLight, Colors.White);

// ❌ Magic string — falha silenciosa em runtime
barColor = ResolveColor("SurfaceDark", Colors.White);
```

**O que não usar:** em XAML use `{StaticResource}` ou `{AppThemeBinding}` diretamente — o XAML parser valida as chaves em tempo de design.

### Routes (`UI/Navigation/Routes.cs`)

Centraliza as strings de rota usadas em `AppShell.xaml` (`Route="{x:Static nav:Routes.Home}"`) e em `MauiNavigationService` para navegação via `GoToAsync`. Ver seção "Navegação para páginas fora da hierarquia visual" acima.

---

## Inicialização do SQLite

O path do arquivo de banco é específico de plataforma (`FileSystem.AppDataDirectory`). Para manter Infrastructure sem dependência de MAUI:

1. **UI** computa o path e registra `IDbContextFactory<AppDbContext>` no DI
2. **Infrastructure** recebe a factory via DI — sem conhecer path ou plataforma
3. **`DatabaseInitializer`** (Infrastructure) aplica migrations no startup via `MigrateAsync()`
