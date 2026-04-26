# 05 — Inventário de Componentes

> Levantamento baseado em `docs/04-screens.md`. A ser revisado e refinado antes de implementar `Styles.xaml`.

---

## ContentViews — estrutura + binding próprios

Cada um vira um arquivo `.xaml` separado em `UI/Controls/`.

### Cards de mídia

| Componente | Estrutura | Usado em |
|---|---|---|
| **CardTitle** | Portrait image + title (truncado) + ★ nota + ano | Listas horizontais, grid 3-col (todas as abas) |
| **CardTrailer** | Landscape 16:9 + play icon overlay + title + date | Últimos Trailers |
| **CardPerson** | Portrait + nome + "known for" (truncado) | Aba Pessoas, resultados de busca |
| **CardCollection** | Portrait + nome (sem rating, sem ano) | Busca — categoria Collections |

### Cards de seção

| Componente | Estrutura | Usado em |
|---|---|---|
| **CardSeason** | Poster + "Season X" + data + nº episódios + ★ rating | Detalhe de Série — Current Season |
| **CardReview** | Avatar + "A review by X" + badge nota + autor + data + trecho | Detalhe de Filme e Série — Reviews |
| **CardLeaderboardItem** | Foto circular + username + 2 progress bars com gradiente | Home — Líderes |
| **CardBenefit** | Ícone teal + texto explicativo | Tela Pré-Login |

### Controles funcionais

| Componente | Comportamento | Usado em |
|---|---|---|
| **ExpanderSection** | Card fechado (título + chevron) / aberto (lista horizontal + botão `todos >`) com animação | Abas Filmes, Séries, Busca |
| **SegmentedControl** | Pill com opção ativa em teal + checkmark, opção inativa em outline | Tendências, Grátis p/ assistir, Settings, Detalhe Pessoa |
| **BottomSheet** | Drag handle + lista de opções, fecha ao arrastar ou tocar fora | Filtros, Find by ID, Image Quality |
| **ProgressRing** | Gráfico circular de progresso (ex: 68%), graduação por cor | Hero de Filme/Série, Perfil, Colaborador |
| **ConnectivityBanner** | Banner global de offline — aparece sobre a UI, some quando volta | Global |

### Estrutura de tela

| Componente | Estrutura | Usado em |
|---|---|---|
| **TopBar** | Ícone usuário (esq) + logo TMDB (centro) + ícone busca M3 (dir) | Todas as abas principais |
| **BottomBar** | ContentView customizado sobreposto via overlay no `AppShell.xaml` — tab bar nativa ocultada via handler override em `MauiProgram.cs` — roteamento via Shell nativo | Global — todas as telas principais |
| **WelcomeBanner** | Card imagem dinâmica + overlay gradiente escuro + título bold + subtítulo | Home |

### Compartilhados

| Componente | Comportamento |
|---|---|
| **SkeletonLoader** | Shimmer no shape do conteúdo que está carregando — estado loading de todas as seções |
| **FullListScreen** | Tela genérica (recebe título + fonte) — CollectionView 3-col + infinite scroll |

---

## Styles — só aparência, sem estrutura própria

Tudo isso vai para `Styles.xaml` com `x:Key`.

### Tipografia — estilos de `Label`

| x:Key | Onde aparece |
|---|---|
| `LabelTitle` | Título de filme/série/pessoa no hero (grande, bold) |
| `LabelSubtitle` | Nomes em cards, títulos de sub-seção |
| `LabelBody` | Sinopse, texto de review, descrições longas |
| `LabelCaption` | Ano, gênero, duração, metadados secundários |
| `LabelTagline` | Tagline do filme (itálico, menor) |
| `LabelSectionHeader` | Títulos de seção dentro de cards (sem badge) |

### Botões — estilos de `Button`

| x:Key | Aparência | Onde aparece |
|---|---|---|
| `ButtonPrimaryPill` | Gradiente brand (teal → azul), texto branco, pill shape | CTAs primários — "Ver Trailer", "Adicionar" |
| `ButtonOutlinePill` | Borda teal, sem fundo, texto teal, pill shape | "Full Cast", "All Reviews", "View Collection", "Login" |
| `ButtonCircularFloating` | Circular, fundo semi-transparente escuro, só ícone | Back (←) e menu (⋮) nas telas de detalhe |
| `ButtonTealFilled` | Fundo teal sólido, texto branco, pill | Estado ativo do toggle (ex: Movies ativo vs TV Shows inativo) |

### Badges — estilos de `Label` ou `Border`+`Label`

| x:Key | Aparência | Onde aparece |
|---|---|---|
| `BadgeSection` | Pill teal preenchido, texto branco, bold | Cabeçalhos de seção nas telas de detalhe |
| `BadgeCount` | Círculo colorido pequeno, número em branco, bold | Contagem de resultados na busca (ex: "275") |
| `BadgeRating` | "★ X.X" — ícone + número pequeno | Cards de título, card de review, card de season |

### Chips/Pills — estilos de `Border`+`Label`

| x:Key | Aparência | Onde aparece |
|---|---|---|
| `ChipMetadata` | Pill borda teal, fundo transparente, texto branco pequeno | Classificação etária, data, duração no hero |
| `ChipGenre` | Pill borda teal, fundo transparente, texto teal | Gêneros no hero (flex wrap), gêneros favoritos |
| `ChipKeyword` | Pill fundo colorido (Surface), texto branco | Seção Keywords, busca — categoria Keywords |
| `ChipStatistic` | Pill teal para vitórias, outline para indicações | Hero de Awards |

### Layout — estilos de `Border`

| x:Key | Aparência | Onde aparece |
|---|---|---|
| `SurfaceCard` | Fundo Surface, bordas arredondadas (~16), sem sombra, sem borda | Cards genéricos, benefit cards, settings items |

### Input — estilo de `Entry`

| x:Key | Aparência | Onde aparece |
|---|---|---|
| `InputM3` | Floating label M3, botão X de limpar interno, bordas arredondadas | Busca, Find by ID |

---

## Resumo

| Categoria | Qtd |
|---|---|
| ContentViews (arquivos `.xaml` próprios) | 18 |
| Estilos de Label (tipografia) | 6 |
| Estilos de Button | 4 |
| Badges | 3 |
| Chips/Pills | 4 |
| Layout (SurfaceCard) | 1 |
| Input | 1 |
| **Total de styles para Styles.xaml** | **19** |

---

## Pontos em aberto (discutir antes de implementar)

- **`BadgeSection`** — Style em dois elementos (`Border`+`Label`) ou ContentView pequeno com binding de `Text`? ContentView é mais semântico.
- **`SurfaceCard`** — Style de `Border` com `AppThemeBinding` no BackgroundColor ou ControlTemplate? Decidir como os usos vão referenciar.
- **`SkeletonLoader`** — shape varia por contexto (card portrait vs card landscape vs linha de texto). Avaliar se é um componente genérico com parâmetros ou múltiplas variantes.
- **`SegmentedControl`** — 2 opções (Today/Week) e 3 opções (Light/System/Dark) — componente precisa suportar N itens ou são variantes separadas?
- **`BottomSheet`** — MAUI não tem nativo. Avaliar implementação custom (ContentView + animação) vs biblioteca.
- **`ProgressRing`** — avaliar se usa `GraphicsView`/`IDrawable` ou uma lib (ex: Microcharts). Confirmar alinhamento com decisão de não usar libs de terceiros para componentes visuais.
- **`BottomBar`** — ContentView sobreposto ao Shell + tab bar nativa oculta via handler. Animação a definir: bolha circular (Navbar-Animation-1) ou pill deslizante (Navbar-Animation-2) — referências RadekVyM. Implementação com `IDrawable` + `GraphicsView` + API MAUI nativa, sem SimpleToolkit.
