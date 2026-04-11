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

### Offline-First

Ver [02-data-flow.md](02-data-flow.md) para detalhes do fluxo.

---

## Inicialização do SQLite

O path do arquivo de banco é específico de plataforma (`FileSystem.AppDataDirectory`). Para manter Infrastructure sem dependência de MAUI:

1. **UI** computa o path e registra `IDbContextFactory<AppDbContext>` no DI
2. **Infrastructure** recebe a factory via DI — sem conhecer path ou plataforma
3. **`DatabaseInitializer`** (Infrastructure) aplica migrations no startup via `MigrateAsync()`
