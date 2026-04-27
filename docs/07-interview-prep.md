# MauiMovies — Overview para Entrevista

> Documento de referência para a apresentação do projeto. Cobre arquitetura, decisões técnicas, fluxo de dados, testes, e o histórico de implementação.

---

## 1. O que é o projeto

App mobile cross-platform em **.NET MAUI** que consome a API pública da **TMDB** (The Movie Database). Construído como portfólio enterprise — foco em **arquitetura limpa**, **testabilidade**, **desacoplamento de camadas** e **práticas modernas do ecossistema .NET**.

**Plataformas alvo:** Android, iOS, MacCatalyst (Windows desabilitado por simplicidade do escopo).

---

## 2. Arquitetura — 3 camadas

```
MauiMovies.UI          ┐
   ↓ depende de        ├─ Composition Root: UI conhece Infra apenas no DI
MauiMovies.Core    ←   ┘
   ↑ implementado por
MauiMovies.Infrastructure
```

### Camadas

| Projeto | Tipo | Responsabilidade |
|---|---|---|
| **MauiMovies.Core** | .NET 9 class library | Domínio, modelos, contratos, casos de uso, ViewModels, Result\<T\> |
| **MauiMovies.Infrastructure** | .NET 9 class library | EF Core/SQLite, DTOs TMDB, HTTP, mappers, repositórios, datasources |
| **MauiMovies.UI** | .NET 9 MAUI | Pages, controles, conversores, navegação, DI Composition Root |

### Regras de dependência (estritas)

- **Core** sem dependências externas (exceto CommunityToolkit.Mvvm)
- **Infrastructure** depende só de Core
- **UI** depende de Core; Infrastructure só no Composition Root (DI/MauiProgram)
- **Nunca** `using MauiMovies.Infrastructure.*` no código de Pages/ViewModels/Controls/Services

### Por que 3 camadas e não Clean Architecture estrita (4)?

Clean Architecture clássica separa Application/Use Cases de Domain. Para um app mobile médio, a sobrecarga de 4 projetos não compensa — Use Cases ficam em `Core/UseCases/` e cumprem o mesmo papel sem o boilerplate.

---

## 3. As 4 representações de dados

```
MovieDto          → Infrastructure/Api/Dtos        contrato JSON da TMDB
MovieEntity       → Infrastructure/Persistence     row do EF/SQLite
Movie             → Core/Entities                  POCO de domínio
MovieModel        → Core/Models                    ObservableObject pra binding XAML
```

**Fluxo de dados completo:**

```
TMDB API
  ↓ HTTP GET trending/all/day
PagedResponseDto<BaseDto>           (Infra/Api)
  ↓ PolymorphicListConverter        (deserializa baseado em "media_type")
List<MovieDto/TvDto/PersonDto>      (Infra/Api/Dtos)
  ↓ MovieDtoMapper.ToDomain()       (Infra/Api/Mapping)
List<Movie/Tv/Person>               (Core/Entities — polimórficos via MediaItem)
  ↓ TmdbMediaDataSource.FetchTrendingAllAsync()
GetTrendingAllUseCase
  ↓ se sucesso: MediaRepository.SaveTrendingAllAsync (transação UPSERT + replace)
  ↓ retorna Result<IReadOnlyList<MediaItem>>
HomeViewModel.LoadTrendingAsync
  ↓ MediaItemModelMapper.ToModel()  (Core/Mapping — switch polimórfico)
ObservableCollection<MediaItemModel>
  ↓ binding XAML
CollectionView + DataTemplateSelector → MovieCard | TvCard | PersonCard
```

### Por que tantas conversões?

Cada camada tem responsabilidades distintas:
- **DTO** — formato JSON da API; pode mudar a qualquer momento (não nosso controle)
- **Entity** — formato SQL/EF; otimizado pra persistência (CSV em vez de listas)
- **Domain** — POCO imutável; representa regra de negócio
- **Model** — `INotifyPropertyChanged` pra UI; com `[ObservableProperty]`

Trocar a fonte de dados (ex: GraphQL no lugar de REST) só afeta DTOs e mappers — domínio fica intocado.

---

## 4. Polimorfismo no Trending

A endpoint `trending/all/day` retorna **lista heterogênea** de Movies + TV Shows + Persons. Como modelar isso?

### Domínio (Core)

```
MediaItem (abstract)
├── Movie     { Title, ReleaseDate, ... }
├── Tv        { Name, FirstAirDate, OriginCountry, ... }
└── Person    { Name, ProfilePath, Gender, KnownForDepartment }
```

`MediaType` é uma propriedade computada (cada subclasse retorna seu próprio tipo) — sem necessidade de campo de discriminator manual.

### Persistência (EF + SQLite)

3 tabelas separadas (não TPH/TPT) + **tabela de associação**:

```
Movies        ┐
TvShows       ├─ entidades por tipo (sem nullable bagunçado)
Persons       ┘

MediaListItems (Id, ListType, MediaType, MediaId, Position)
  - sem FK explícita (MediaId resolve dinamicamente conforme MediaType)
  - índice (ListType, Position) pra leitura ordenada
  - índice único (ListType, MediaType, MediaId) — evita dupes no mesmo cache
```

**Por que sem FK?** Ter 3 FKs nullable seria feio e geraria constraints malucas ("exatamente um deve ser preenchido"). Como a junction é puro cache, a integridade é garantida pelo código no save (transação).

### UI

`ObservableCollection<MediaItemModel>` + `MediaItemTemplateSelector` que escolhe o `DataTemplate` certo (`MovieCard`, `TvCard`, `PersonCard`) baseado no tipo runtime do item.

---

## 5. Cache offline-first

Decisão fechada: **network-first com fallback offline**, sem TTL.

```csharp
// GetTrendingAllUseCase.ExecuteAsync
try
{
    var items = await dataSource.FetchTrendingAllAsync(ct);  // HTTP
    await repository.SaveTrendingAllAsync(items, ct);         // cache
    return Result<...>.Success(items);
}
catch
{
    var cached = await repository.GetTrendingAllAsync(ct);    // SQLite
    if (cached.Count > 0)
        return Result<...>.Success(cached);
    return Result<...>.Fail("No trending data available offline");
}
```

- Online → sempre busca API e atualiza cache
- Offline → lê SQLite
- Cache cresce orgânico (paginação) — sem expiração de TTL
- Cards de imagem têm cache HTTP do MAUI separadamente

---

## 6. Composition Root (DI)

UI conhece Infrastructure **apenas no DI wiring**, nunca no código.

```csharp
// MauiProgram.cs (single touchpoint UI ↔ Infrastructure)
var configuration = LoadConfiguration();   // appsettings.json embedded
var tmdbOptions = configuration.GetSection("Tmdb").Get<TmdbOptions>();
var sqlitePath = Path.Combine(FileSystem.AppDataDirectory, "mauimovies.db");

builder.Services
    .AddInfrastructure(sqlitePath, tmdbOptions)  // Infrastructure/DI/InfrastructureExtensions
    .AddUI();                                     // UI/DI/UIExtensions
```

**Disciplina:** code review garante que `Pages/`, `ViewModels/`, `Controls/`, `Services/` usem só `Core` — `using MauiMovies.Infrastructure` só em `MauiProgram.cs` e `DI/`.

### Lifetimes

- `IRequestProvider`, `DatabaseInitializer` → **Singleton** (stateless)
- `IDbContextFactory<AppDbContext>` → **Singleton** (factory pattern)
- ViewModels, Pages, Use Cases, Repositories, DataSources → **Transient** (MAUI não cria scopes; Transient é o pattern correto)

---

## 7. Banco SQLite + EF Core

- **`IDbContextFactory<AppDbContext>`** — nunca injetar `AppDbContext` direto (thread-safety)
- **Fluent API** em `Persistence/Configurations/` — sem Data Annotations
- **Migrations** geradas via `dotnet ef` (tool local em `dotnet-tools.json`)
- **`DesignTimeDbContextFactory`** — permite gerar migrations sem rodar a app
- **`DatabaseInitializer.MigrateAsync`** rodando no `App.OnStart`

### Schema atual

```sql
CREATE TABLE Movies (Id INTEGER PRIMARY KEY, Title TEXT, ..., GenreIds TEXT)
CREATE TABLE TvShows (Id INTEGER PRIMARY KEY, Name TEXT, ..., OriginCountry TEXT)
CREATE TABLE Persons (Id INTEGER PRIMARY KEY, Name TEXT, ProfilePath TEXT, Gender INTEGER, ...)
CREATE TABLE MediaListItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    ListType INTEGER NOT NULL,
    MediaType INTEGER NOT NULL,
    MediaId INTEGER NOT NULL,
    Position INTEGER NOT NULL
)
CREATE INDEX IX_MediaListItems_ListType_Position ON MediaListItems (ListType, Position)
CREATE UNIQUE INDEX IX_MediaListItems_ListType_MediaType_MediaId ON MediaListItems (ListType, MediaType, MediaId)
```

`GenreIds` (filme/tv) e `OriginCountry` (tv) armazenados como **CSV** — escolha pragmática para evitar tabelas extras de junção (nunca consultamos por gênero, só renderizamos chips).

---

## 8. Testes

### Estratégia

| Camada | Framework | Estratégia |
|---|---|---|
| **Core** | xUnit + Moq | Unit tests puros — mock `IMediaRepository`, `IMediaRemoteDataSource` |
| **Infrastructure** | xUnit + SQLite in-memory | **Integração real** com EF Core — nunca mockar o banco |
| **UI** | Appium (planejado) | E2E no dispositivo, fase futura |

### Testes implementados (56/56 passando)

**Core.Tests (24 testes):**
- `Common/ResultTests` — factory methods Success/Fail
- `Mapping/*ModelMapperTests` — Domain → Model com null safety
- `UseCases/GetTrendingAllUseCaseTests` — sucesso, fallback offline, fail propagation, não consulta cache quando online
- `ViewModels/HomeViewModelTests` — OnAppearing lifecycle, polymorphic loading, error state, WelcomeBackdropPath random

**Infrastructure.Tests (32 testes):**
- `Api/Mapping/*DtoMapperTests` — DTO → Domain com null fallbacks, Theory data para Gender
- `Persistence/Mapping/*EntityMapperTests` — round-trip Entity ↔ Domain incluindo CSV
- `Persistence/Repositories/MediaRepositoryTests` — **integração real com SQLite in-memory**: persiste polimorfismo, replace de lista (delete + insert), UPSERT, dedupe na tabela Movies, ordem por Position

### Pattern do `DatabaseTestBase`

`SqliteConnection` em `:memory:` mantida aberta durante o ciclo do teste. xUnit cria nova instância do test class por teste = isolamento total.

---

## 9. Decisões técnicas relevantes

| Decisão | Por quê |
|---|---|
| 3 camadas em vez de 4 | Application = Use Cases dentro de Core. Menos boilerplate. |
| `Result<T>` próprio (sem libs) | Use Cases capturam exceção → convertem em `Result.Fail`. ViewModel sempre verifica `IsSuccess`. |
| Polymorphic via MediaItem abstract | Domain limpo. Persistence resolve via tabela de associação sem FK. |
| `IDbContextFactory` em vez de `AppDbContext` injetado | Thread-safety. Cada operação cria/dispõe contexto. |
| ViewModels no Core (não na UI) | Testáveis com xUnit puro. UI fica como camada thin. |
| CommunityToolkit.Mvvm | `[ObservableProperty]` + `[RelayCommand]` — código bem mais conciso. |
| Composition Root em UI | UI referencia Infrastructure só pra DI. Code review garante separação. |
| SQLite in-memory pros testes de Infra | Integração REAL — schema + migrations + queries idênticos a produção. |
| Bearer token TMDB v4 | JWT que funciona em endpoints v3. Configurável em `appsettings.json` (gitignored). |

---

## 10. Histórico de commits da feature trending

```
test(infra): add DTO/Entity mapper tests and MediaRepository integration tests
test(core): add unit tests for Result, mappers, GetTrendingAllUseCase and HomeViewModel
feat(ui): integrate HomeTabView with welcome banner and trending CollectionView via DI
feat(ui): add polymorphic cards, converters, template selector and i18n resources
feat: wire DI Composition Root with appsettings and DatabaseInitializer on startup
feat(infra): implement TMDB DataSource, RequestProvider GET and polymorphic MediaRepository
feat(infra): adjust DTOs for trending response and add DTO/Entity mappers
feat(infra): set up EF Core SQLite schema with polymorphic media tables and migrations
feat(core): add GetTrendingAllUseCase with offline fallback and HomeViewModel
feat(core): add Result<T> and media repository/datasource contracts
feat(core): add observable UI models and Domain-to-Model mappers
feat(core): add MediaItem polymorphic hierarchy with Movie/Tv/Person and shared enums
chore: install dotnet-ef as local tool and ignore appsettings.json
```

---

## 11. Pontos para a entrevista — perguntas prováveis e como responder

### "Por que 3 camadas?"
> Application Service Layer (Use Cases) costuma viver em projeto separado em Clean Architecture estrita. Para um app mobile com complexidade média, mantê-los dentro de Core elimina overhead sem perder testabilidade nem desacoplamento.

### "Como você lidaria com a heterogeneidade do trending?"
> Hierarquia polimórfica no domínio (`MediaItem` abstract → `Movie`, `Tv`, `Person`). Na persistência, 3 tabelas + tabela de associação `MediaListItems` que resolve `MediaId` dinamicamente conforme `MediaType`. Na UI, `DataTemplateSelector` escolhe o card certo baseado no tipo runtime do item na `ObservableCollection<MediaItemModel>`.

### "Como funciona o cache offline?"
> Network-first: tenta API → se sucesso, salva cache + retorna; se fail, lê cache → se vazio, retorna `Result.Fail`. Sem TTL — cache cresce orgânico por paginação. Dentro do Use Case, captura única que decide retornar resultado novo ou cached.

### "Como está testado?"
> Core: 24 unit tests com xUnit + Moq. Infrastructure: 32 testes incluindo **integração real com SQLite in-memory** — sem mocks de banco. Total 56/56 passando.

### "Como funciona o DI?"
> Composition Root no `MauiProgram.cs`. UI tem ProjectReference pra Infrastructure mas só usa essas classes no DI wiring (`InfrastructureExtensions.cs`). Code review garante que Pages/ViewModels só usem Core abstractions.

### "Migrations EF Core num app móvel?"
> `dotnet-ef` instalado como tool local (`dotnet-tools.json` versionado). `DesignTimeDbContextFactory` permite gerar migrations sem rodar a app. `DatabaseInitializer.MigrateAsync` roda no `App.OnStart` — primeira execução cria schema, atualizações futuras aplicam migrations pendentes.

### "Como funciona o status bar dinâmico?"
> Override de `Application.OnStart` + listener de `RequestedThemeChanged`. Usa `CommunityToolkit.Maui.Core.Platform.StatusBar` pra setar cor + style baseado no tema. `StatusBarBehavior` XAML não responde a runtime theme changes — precisou ser code-behind.

### "Por que MediaListItems sem FK?"
> Junção polimórfica — uma só FK não funciona, e 3 FKs nullable + constraint "exatamente uma" é feio. Como a tabela é puro cache (não fonte de verdade), integridade é garantida no código (transação no save). Trade-off consciente.

---

## 12. O que ficou pra próxima sessão

Ver `memory/todo.md` no contexto. Principais pendências documentadas como issues majors da auditoria pré-commit.

---

*Documento gerado durante implementação 2026-04-27 → atualizado em cada fase de implementação seguinte.*
