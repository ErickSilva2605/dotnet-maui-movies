# Estratégia de Testes

## Princípio Central

Toda lógica de negócio deve ser testável com **xUnit puro**, sem emulador, sem device, sem rodar um app. Testes de UI (Appium) cobrem fluxos do usuário de ponta a ponta.

Isso define onde cada componente mora: **ViewModels e Use Cases vivem no Core** (`.NET Class Library`), não no projeto MAUI.

---

## Pirâmide de Testes

```
          ┌─────────────┐
          │   Appium    │  ← fluxos de usuário, navegação, visual
          │  (UI Tests) │
          └──────┬──────┘
         ┌───────┴────────┐
         │  Infrastructure │  ← repositórios, data sources, mappers
         │     Tests       │
         └───────┬─────────┘
        ┌────────┴──────────┐
        │    Core Tests     │  ← use cases, viewmodels, lógica (maior cobertura)
        └───────────────────┘
```

---

## Projetos de Teste

### MauiMovies.Core.Tests

**Ferramenta:** xUnit + Moq

**O que testa:**
- Use Cases (lógica de negócio e orquestração offline-first)
- ViewModels (estados, comandos, condicionais, edge cases)
- Mappers Core (domain entity → model)

**Características:**
- Zero dependência de MAUI
- Zero dependência de banco de dados real
- Zero dependência de HTTP real
- Roda em qualquer terminal ou CI sem configuração adicional
- Todas as dependências externas são mockadas

**Exemplo:**
```csharp
[Fact]
public async Task GetTrending_WhenRemoteFails_ReturnsCachedData()
{
    _remoteSource.Setup(x => x.GetTrendingAsync())
                 .ReturnsAsync(Result.Failure("sem conexão"));
    _localRepo.Setup(x => x.GetTrendingAsync())
              .ReturnsAsync(new List<Movie> { new() { Title = "Inception" } });

    var result = await _useCase.ExecuteAsync();

    Assert.Single(result);
    Assert.Equal("Inception", result[0].Title);
}
```

---

### MauiMovies.Infrastructure.Tests

**Ferramenta:** xUnit + SQLite in-memory + WireMock.Net (ou HttpClient mock)

**O que testa:**
- Repositórios EF Core (CRUD, queries, relacionamentos)
- DataSources TMDB (desserialização, mapeamento de resposta)
- Mappers de infraestrutura (DTO → Entity, Entity → Domain)
- DatabaseInitializer (criação e migration de tabelas)

**Características:**
- SQLite em memória (`:memory:`) — sem arquivo físico, sem plataforma
- HTTP mockado — sem chamadas reais à API TMDB
- Testa integração real entre Repository e EF Core

**Exemplo:**
```csharp
public class MovieRepositoryTests : IAsyncLifetime
{
    private AppDbContext _context;
    private MovieRepository _sut;

    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        _context = new AppDbContext(options);
        await _context.Database.EnsureCreatedAsync();
        _sut = new MovieRepository(new TestDbContextFactory(options));
    }

    [Fact]
    public async Task GetTrendingAsync_ReturnsOnlySavedMovies()
    {
        _context.Movies.Add(new MovieEntity { Id = 1, Title = "Dune" });
        await _context.SaveChangesAsync();

        var result = await _sut.GetTrendingAsync();

        Assert.Single(result);
        Assert.Equal("Dune", result[0].Title);
    }

    public async Task DisposeAsync() => await _context.DisposeAsync();
}
```

---

### MauiMovies.UITests.*

**Ferramenta:** Appium WebDriver

**Plataformas:** Android, iOS, macOS, Windows (projetos separados)

**O que testa:**
- Fluxos de navegação completos
- Interações do usuário (scroll, tap, swipe)
- Exibição correta de dados na tela
- Comportamento offline (telas de erro, cache)
- Acessibilidade básica

**O que NÃO testa:**
- Lógica de negócio (coberta pelo Core.Tests)
- Queries de banco (cobertas pelo Infrastructure.Tests)
- Edge cases de estado (cobertos pelo Core.Tests)

**Características:**
- Testes lentos por natureza — reservados para fluxos críticos
- Requerem device ou emulador
- Executados separadamente do pipeline de testes unitários

---

## O que NÃO usamos e por quê

| Ferramenta | Motivo |
|---|---|
| **TestDevices** | Requer rodar um app MAUI para executar testes — viola o princípio de testes rápidos sem plataforma |
| **Mocks de banco no Core** | Core não conhece banco. Repositories são mockados via interface |
| **Chamadas reais à API TMDB nos testes** | Testes não devem depender de serviços externos |

---

## Cobertura Esperada por Camada

| Camada | Tipo de Teste | Cobertura Alvo |
|---|---|---|
| Core — Use Cases | xUnit unitário | Alta |
| Core — ViewModels | xUnit unitário | Alta |
| Infrastructure — Repositories | xUnit integração (SQLite in-memory) | Alta |
| Infrastructure — DataSources | xUnit + HTTP mock | Média |
| UI — Fluxos principais | Appium | Fluxos críticos |

---

## Convenções

- Nomenclatura de testes: `MetodoTestado_Cenario_ResultadoEsperado`
- Um `Assert` principal por teste
- Arrange / Act / Assert explícitos
- Mocks criados no construtor da classe de teste ou via `[SetUp]`
- Dados de teste isolados por teste — sem estado compartilhado entre testes
