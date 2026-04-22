# 04 — Mapeamento de Telas

Registro das telas, abas, seções e decisões de UI/UX do MauiMovies.

---

## Detalhe de Filme

> Base de referência: app TMDB mobile. Não replicar literalmente — usar como estrutura e adaptar visualmente.
> Navegações desta tela a serem validadas após implementação.

### Navegação
- Página empilhada na stack da aba de origem — sem tab bar inferior
- Botões flutuantes sobrepostos ao backdrop (não é uma TopBar): `←` voltar (esquerda) + `⋮` menu (direita), em botões circulares

### 1. Hero
- Backdrop do filme em largura total (~40% superior da tela)
- **Efeito parallax:** backdrop permanece visível atrás de todas as seções durante o scroll
- Sobre o backdrop (de cima pra baixo):
  - **Poster** (portrait, topo, bordas arredondadas)
  - **Título** (grande, branco, bold)
  - **Chips de metadados:** classificação etária · data de lançamento (ícone calendário) · duração (ícone relógio)
  - **Gráfico circular de avaliação** (progress ring — ex: 68%) substituindo o chip de rating
  - **Chips de gênero** (pills teal outline — ex: Drama, Crime) — flex wrap, quebra linha quando não cabe
  - **Tagline** em itálico

### 2. Ações do usuário *(requer login — seção inteira oculta quando não logado)*
- **Visibilidade condicional:** a seção inteira (card + Vibes) não é exibida para usuários não autenticados — não é desabilitada, é removida do layout
- Card escuro com 4 botões circulares + label abaixo de cada:
  - **Lists** · **Favorite** · **Watchlist** · **Rate**
- **Vibes** — sistema de votação emocional:
  - Botões de emoji representando humor do filme
  - Ao tocar: abre modal com régua 0–100, seleção de humor (emojis), campo "Tom" (autocomplete com opções do TMDB) e campo "Esse filme foi" (texto com autocomplete)

### 3. Overview
- Badge teal "Overview"
- Sinopse truncada (~3 linhas) com `▾` para expandir / `▴` para recolher
- Animação suave de expansão/colapso

### 4. About movie
- Badge teal "About movie"
- Linha de links externos: IMDb, Facebook, Twitter, Instagram, Website
- Tabela de informações: Status · Idioma original · Orçamento · Receita · País de produção
- **Produtoras:** chips empilhados (pill teal)

### 5. Mídia
- Tabs inline: **Videos** | **Posters** | **Backdrops**
- Videos: scroll horizontal de thumbnails (play + título + data)
- Botão "All Videos" (pill outline)
- ⚠️ **Comportamento de card:** ao trocar de aba, a altura do card anima suavemente (expand/collapse) para acomodar o novo conteúdo — implementar no componente de card genérico

### 6. Top Billed Cast
- Badge teal "Top Billed Cast"
- Scroll horizontal: foto (portrait) + nome (bold) + personagem (cor secundária)
- Botão "Full Cast & Crew" (pill outline)

### 7. Coleção *(condicional — só exibe se o filme pertence a uma coleção)*
- Card: poster da coleção + nome da coleção
- Botão "View the Collection" (pill outline)

### 8. Onde assistir — JustWatch
- Badge teal "Available on JustWatch 🇧🇷" (bandeira do país do usuário)
- Organizado por tipo (linhas separadas): Ads · Buy · Rent · Stream
- Logos das plataformas em cada linha

### 9. Reviews
- Badge teal "Reviews"
- Card de destaque: avatar + "A review by [nome]" + badge de nota (★ X.X) + autor + data + trecho
- Botão "Read All Reviews (N)" (pill outline)

### 10. Release Dates
- Badge teal "Release Dates 🇧🇷" (bandeira do país)
- Scroll horizontal de cards: data + classificação etária + tipo (Theatrical/Physical) + formato
- Botão "More" (pill outline)

### 11. Recomendações / Similares
- Tabs inline: **Recommendations** | **Similar movies**
- Scroll horizontal de cards de filme: poster + título + ★ nota + ano

### 12. Keywords
- Badge teal "Keywords"
- Flow wrap de chips clicáveis

### 13. Avaliação do conteúdo
- Badge teal "Avaliação do conteúdo"
- Score de qualidade da página (ex: 100 — Excelente) — gerado pela comunidade TMDB

### 14. Principais colaboradores
- Badge teal "Principais colaboradores"
- Grid dos 4 maiores contribuidores da página: avatar + username + nº de edições

### 15. Curva de popularidade
- Badge teal "Curva de popularidade"
- Gráfico de linha simples mostrando a evolução do índice de popularidade ao longo do tempo

---

### Padrões visuais da tela

| Elemento | Padrão |
|---|---|
| Títulos de seção | Badge/pill teal preenchido, texto branco |
| Botões "ver mais" | Pill outline, sem preenchimento |
| Chips de metadados | Pills teal ou coloridos (rating: laranja) |
| Fundo dos cards | `Surface` (`#0D253F` dark) |
| Backdrop | Parallax — persiste atrás de todo o conteúdo |
| Animação de card | Altura anima suavemente ao trocar conteúdo interno |

---

## Detalhe de Série

> Mesma estrutura base do Detalhe de Filme. Documentadas aqui apenas as diferenças e seções exclusivas.
> Navegações desta tela a serem validadas após implementação.

### Navegação
- Idêntica ao Detalhe de Filme: página empilhada na stack, botões flutuantes `←` e `⋮`

### Hero — diferenças
- **Classificação etária:** presente nos dois (Filme e Série) quando a API entregar o dado — mesmo chip no hero
- **Sem chip de duração** — episódios têm duração variável, não faz sentido para série
- **Chips de gênero:** layout flex wrap — quando o texto não cabe na mesma linha, o chip quebra para a linha seguinte naturalmente (não é empilhamento intencional)
- **Gráfico circular de avaliação**, Vibes, Avaliação do conteúdo, Principais colaboradores e Curva de popularidade: idênticos ao Filme

### About TV Show *(em vez de "About movie")*

Campos **removidos** em relação ao Filme: Budget · Revenue

Campos **exclusivos** de Série:

| Campo | Observação |
|---|---|
| Creator | Nome do criador da série |
| Network | Exibido como **logo/imagem** da emissora, não texto |
| Type | Ex: Scripted, Reality, Documentary |
| Number of seasons | Total de temporadas |
| Number of episodes | Total de episódios |

### Seção exclusiva — Current Season
- Badge teal "Current Season"
- Card: poster da temporada + "Season X" + data de estreia + nº de episódios + chip de avaliação (★)
- Subseção **Next Episode:** número + título do episódio + data de estreia
- Botão "View All Seasons" (pill outline)

### Series Cast *(em vez de "Top Billed Cast")*
- Badge "Series Cast"
- Cards com info extra: foto + nome + personagem + **(N episodes)**

### Recomendações / Similares
- Segunda aba: "**Similar TV Shows**" em vez de "Similar movies"

### Seções idênticas ao Filme
Overview · Ações do usuário (oculta quando não logado) · Mídia (Videos/Posters/Backdrops) · JustWatch · Reviews · Keywords

---

## Detalhe de Pessoa

> Tela significativamente mais simples que Filme e Série — sem hero, sem ações de usuário, sem JustWatch.
> Navegações desta tela a serem validadas após implementação.

### Navegação
- Mesmos botões flutuantes `←` e `⋮`
- **Sem backdrop/hero** — fundo escuro puro, sem imagem de fundo
- Sem tab bar inferior (página empilhada)

### 1. Header
- **Foto** da pessoa (portrait, bordas arredondadas, lado esquerdo)
- **Nome** (grande, bold, lado direito)
- **Links sociais** ao lado do nome: IMDb, Twitter, Instagram *(quantidade varia por pessoa)*
- **Tabela de informações:**
  - Known For (área de atuação — ex: Acting)
  - Gender
  - Birthday: data + **(N years old)** — idade calculada
  - Place of Birth

### 2. Biography
- Badge teal "Biography"
- Texto truncado com `▾` para expandir / `▴` para recolher — mesmo padrão do Overview

### 3. Profiles
- Badge teal "Profiles"
- Scroll horizontal de fotos da pessoa (diferentes sessões/eventos)

### 4. Known For
- Badge teal "Known For"
- Contagem de créditos por tipo em grid 2 colunas (ex: "Acting: 53" · "Production: 14")
- Toggle de filtro: **Movies** (botão teal preenchido, ativo) | **TV Shows** (botão outline, inativo)
- Lista de créditos filtrada abaixo do toggle

### O que NÃO tem em relação a Filme/Série
Ações do usuário · JustWatch · Reviews · Recomendações · Keywords · Release dates · Coleção · Gêneros · Rating

---

## Detalhe de Colaborador

> Segue o mesmo padrão visual da tela de Detalhe de Pessoa.
> Sem backdrop — fundo escuro puro. Ajustes finos na implementação.

### Navegação
- Página empilhada a partir do Leaderboard ou de qualquer menção de colaborador
- Botões flutuantes `←` e `⋮` — sem backdrop, fundo escuro puro

### 1. Header
- **Foto** portrait com bordas arredondadas (lado esquerdo) — mesmo padrão da Pessoa
- Se sem foto: placeholder no mesmo formato retangular arredondado com inicial do username
- **Username** (grande, bold, lado direito)
- **"Membro desde [mês/ano]"** (texto secundário)

### 2. Estatísticas principais
- Badge teal "Estatísticas"
- Dois **gráficos circulares de progresso** lado a lado:
  - Total de edições
  - Total de avaliações
- Número total como label abaixo de cada gráfico

### 3. Gêneros favoritos
- Badge teal "Gêneros favoritos"
- Chips dos gêneros mais assistidos/avaliados — flex wrap

### 4. Atividade recente
- Badge teal "Atividade recente"
- Lista de edições recentes: tipo (série/filme/pessoa) + título + data + nº de edições
- Agrupado por data

### 5. Gráfico de atividade — últimos 30 dias
- Badge teal "Atividade (30 dias)"
- Gráfico de linha simples mostrando volume de edições por dia

### 6. Listas públicas
- Badge teal "Listas públicas"
- Grid de cards: nome da lista + nº de itens + data de criação
- Botão "Ver todas" (pill outline)

---

## Perfil do usuário

> Similar à tela de Detalhe de Colaborador — mesma estrutura base, adaptada para o perfil próprio.
> Acesso: botão `👤` na top bar (usuário logado). Sem login → redireciona para Login.
> Ajustes finos na implementação.

### Navegação
- Página empilhada a partir da top bar
- `←` voltar (esquerda) + ícone de **logout** vermelho (direita) — em vez de `⋮`
- Sem backdrop — fundo escuro puro

### 1. Header
- **Foto** portrait com bordas arredondadas (lado esquerdo) — mesmo padrão do Colaborador
- Se sem foto: placeholder retangular arredondado com inicial do username
- **Username** (grande, bold, lado direito)
- **"Membro desde [mês/ano]"** (texto secundário)

### 2. Estatísticas principais
- Badge teal "Estatísticas"
- 4 métricas em grid 2×2:
  - Avaliação média — Filmes (gráfico circular de progresso)
  - Avaliação média — Séries (gráfico circular de progresso)
  - Total de avaliações (número)
  - Total de edições (número)

### 3. Ações do usuário *(hub exclusivo do perfil próprio)*
- 4 itens de menu em cards grandes, com ícone teal + label + chevron `›`:
  - **Lists** — abre tela de listas com botão "+ New list"
  - **Favorites** — abre tela com tabs Movies / TV Shows + grid 3 colunas (cards com lixeira)
  - **Watchlist** — idem Favorites
  - **Ratings** — idem Favorites

### 4. Atividade recente
- Badge teal "Atividade recente"
- Lista de edições recentes: tipo + título + data + nº de edições — igual ao Colaborador

### 5. Gráfico de atividade — últimos 30 dias
- Badge teal "Atividade (30 dias)"
- Gráfico de linha simples — igual ao Colaborador

### Diferenças em relação ao Detalhe de Colaborador
| Elemento | Colaborador | Perfil próprio |
|---|---|---|
| Botão direito | `⋮` | Logout (vermelho) |
| Stats | Edições + Avaliações | Avg filmes + Avg séries + Avaliações + Edições |
| Ações | — | Hub: Lists / Favorites / Watchlist / Ratings |
| Gêneros favoritos | Sim | Não |
| Listas públicas | Sim | Substituído pelo hub de ações |

---

## Detalhe de Awards

> Segue o mesmo padrão visual das outras telas de detalhe (Filme, Série, Pessoa).
> Referência de conteúdo: site TMDB (`/movie/{id}/awards`, `/tv/{id}/awards`).
> Navegações desta tela a serem validadas após implementação.

### Navegação
- Página empilhada a partir da aba Awards
- Botões flutuantes `←` e `⋮` sobrepostos ao hero

### 1. Hero
- Backdrop ou imagem representativa da premiação em largura total
- **Efeito parallax** — mesmo padrão de Filme/Série
- Sobre o hero:
  - **Logo/imagem da premiação** (portrait, topo, bordas arredondadas — no lugar do poster)
  - **Nome da premiação** (grande, bold — ex: Academy Awards)
  - **Chips de metadados:** país de origem · ano de fundação (quando disponível)
  - **Chips de estatística:** `N vitórias` (teal) · `N indicações` (outline)

### 2. Seletor de Cerimônia
- Badge teal "Cerimônias"
- Scroll horizontal de chips por edição/ano — chip da cerimônia mais recente ativo por padrão
- Ao selecionar: conteúdo abaixo atualiza com animação

### 3. Categorias da cerimônia selecionada
- Para cada categoria (ex: "Best Picture"):
  - Badge teal com nome da categoria
  - **Vencedor** em destaque: card maior com ícone de troféu, poster/foto + título ou nome
  - **Demais indicados:** scroll horizontal de cards menores — poster/foto + título/nome
  - Status visual claro: vencedor vs indicado

### 4. Avaliação do conteúdo
- Idêntico ao Filme — score de qualidade da página TMDB

### 5. Principais colaboradores
- Idêntico ao Filme — grid dos 4 maiores contribuidores

---

## Configurações

> Acesso: dentro da tela de Perfil (ícone `👤` na top bar).
> Tela simples sem hero — fundo Background puro.
> Referência: app TMDB mobile (config_1 a config_4).

### Navegação
- `←` voltar (esquerda)
- Sem botão de ação à direita
- Título "Settings" fixo no topo

### Tela principal — Settings

Estrutura em seções com badge teal. Cada seção pode ter: item simples com chevron (navega para sub-tela), toggle ou controle segmentado.

#### Theme
- Badge teal "Theme"
- Toggle **"Use dynamic colors"** (ON/OFF)
  - Quando ON: usa cores dinâmicas do sistema (Android 12+)
  - Quando OFF: exibe o controle segmentado de tema manual
- **Controle segmentado:** `☀ Light` | `⚙ System` (padrão ativo) | `🌙 Dark`
  - Ícone + label em cada opção
  - Opção ativa: pill teal preenchido

#### Country
- Badge teal "Country"
- Linha superior: emoji flag + nome do país (ex: "🇧🇷 Brazil") + chevron `›` → abre sub-tela de seleção
- Linha inferior: toggle **"Use country as filter by default"** (ON)

#### Language
- Badge teal "Language"
- Idioma atual (ex: "English") + chevron `›` → abre sub-tela de seleção
- Nota informativa abaixo: explicação de que a mudança afeta apenas dados da API (ex: sinopses, títulos)

#### Image Quality
- Badge teal "Image Quality"
- Qualidade selecionada (ex: "Original") + chevron `›` → abre bottom sheet de seleção

#### Footer
- "Version: X.X.X" — número da versão do app
- Texto de disclaimer/atribuição do TMDB

---

### Comportamento por estado de autenticação *(a resolver — ver TODOs)*

Rascunho inicial antes do mapeamento por screenshots. Os itens abaixo precisam ser revisados contra o que a API cobre e a decisão de acesso às configurações fora do perfil.

| Item | Não autenticado | Autenticado |
|---|---|---|
| Theme (dark mode / idioma) | ✅ sempre visível | ✅ sempre visível |
| Country | ✅ | ✅ |
| Language | ✅ | ✅ |
| Image Quality | ✅ | ✅ |
| Perfil | — | editar dados do perfil TMDB |
| Notificações | — | preferências |
| Conta | — | email, senha, dados |
| Sobre | ✅ | ✅ |
| CTA login/cadastro | ✅ (entrar / criar conta) | — |
| Botão "Sair" | — | ✅ |

> ⚠️ Acesso a Configurações para usuários não logados ainda não resolvido — ver TODOs.

---

### Sub-tela — Country picker

- `←` voltar + botão **"Apply"** (pill outline, canto superior direito)
- Lista alfabética de países com sticky headers de letra (A, B, C...)
- Cada item: emoji flag + nome do país
- Ao selecionar: item marcado visualmente, confirmar com "Apply"

---

### Sub-tela — Language picker

- Idêntica ao Country picker em estrutura: `←` + "Apply"
- Lista alfabética de idiomas com sticky headers de letra
- Apenas nome do idioma — sem flag

---

### Bottom Sheet — Image Quality

- Sobreposto à tela de Settings (Settings visível atrás, escurecida)
- Lista de opções: **Low** · **Medium** · **High** · **Original**
- Item selecionado: row destacada (fundo Surface) + checkmark azul à direita
- Seleção imediata — sem botão "Apply"

---

## Tela Pré-Login (Gateway)

> Exibida ao tocar no ícone `👤` quando o usuário não está autenticado.
> Esta é uma tela de apresentação de valor — não é o formulário de login em si.
> Referência: app TMDB mobile (exemplo_pre-login.jpeg).

### Navegação
- `←` voltar (canto superior esquerdo, botão circular)
- Sem top bar — fundo Background escuro puro
- Página empilhada a partir da top bar

### Estrutura

#### Ilustração
- Ilustração isométrica grande centralizada no topo (arte decorativa — não interativa)
- Estilo flat/isométrico, cores alinhadas à paleta do app

#### Título
- **"Login to your account"** — grande, bold, centralizado

#### Cards de benefícios
- 4 cards com fundo Surface, bordas arredondadas, espaçados verticalmente
- Cada card: ícone teal (esquerda) + texto explicativo (direita)
- Benefícios listados:
  1. ⭐ Rate movies and TV shows
  2. ♥ Manage your watch- and favorite lists
  3. ☰ Create, update and manage your personal lists
  4. 🔥 Get access to personal movies and TV shows recommendations

#### CTA principal
- Botão **"Login"** — pill outline larga, largura total, texto teal bold
- Sem preenchimento — só borda

#### CTA secundário
- Linha de texto: "Don't have an account? **Join TMDB**, it's free and simple."
- "Join TMDB" é link teal clicável → ação a definir (ver TODOs)

---

## Login e Cadastro

> Telas ainda não mapeadas — ver TODOs para pontos de decisão antes de implementar.

---

## Status das telas

| Tela | Status |
|---|---|
| Home | ✅ Definida |
| Filmes | ✅ Definida |
| Séries | ✅ Definida |
| Pessoas | ✅ Definida |
| Awards (lista) | ⚠️ Estrutura básica — detalhe não mapeado |
| Busca M3 (comportamento + conteúdo) | ✅ Definida |
| Tela de lista completa (shared) | ✅ Definida |
| Placar de Colaboradores (Leaderboard) | ✅ Definida |
| Detalhe de Colaborador | ⚠️ Estrutura mapeada — ajustes finos na implementação |
| Detalhe de Filme | ⚠️ Estrutura mapeada — navegações a validar após implementação |
| Detalhe de Série | ⚠️ Estrutura mapeada — navegações a validar após implementação |
| Detalhe de Pessoa | ⚠️ Estrutura mapeada — navegações a validar após implementação |
| Detalhe de Awards (premiação específica) | ⚠️ Estrutura mapeada — navegações a validar após implementação |
| Perfil do usuário (tela/drawer ao clicar em 👤) | ⚠️ Estrutura mapeada — ajustes finos na implementação |
| Tela Pré-Login (gateway ao tocar em 👤 sem estar logado) | ⚠️ Estrutura mapeada — decisões de login/cadastro pendentes |
| Login (formulário interno) | ❌ A definir — ver TODOs |
| Cadastro | ❌ A definir — validar suporte da API TMDB |
| Configurações | ⚠️ Estrutura mapeada — ajustes finos na implementação |

---

## Pendências de design/tech

| Item | Status |
|---|---|
| Biblioteca de ícones | ✅ Lucide Icons — 1.695 ícones, `.ttf` oficial via `lucide-static` |
| Fontes | ✅ Source Sans 3 — Light (300), Regular (400), SemiBold (600) |
| Paleta de cores light/dark mode | ✅ Definida (ver seção abaixo) |
| Bottom tabs: nativo vs. custom renderer | ✅ Decidido — ver `docs/01-architecture.md` |

---

## Paleta de cores

> **Regra estrutural:** `TopBar = Background` / `BottomBar = Surface` — em ambos os modos.
> O contraste entre Background e Surface no dark pode ser refinado durante o design das telas.

### Base / Estrutura

Camadas da interface. Cada nível tem um tom ligeiramente diferente para criar profundidade visual sem usar sombras pesadas.

| Token | Light | Dark | Onde usar |
|---|---|---|---|
| `Background` | `#FFFFFF` | `#031D33` | Fundo de todas as páginas |
| `Surface` | `#F4F4F4` | `#0D253F` | Cards de filme/série/pessoa, listas, bottom sheets, modais |
| `TopBar` | `#FFFFFF` | `#031D33` | Barra superior — mesma cor do Background, fundida com a tela |
| `BottomBar` | `#F4F4F4` | `#0D253F` | Barra de navegação inferior — mesma cor da Surface, levemente elevada |

### Texto

Texto sempre sobre Background ou Surface — nunca sobre cores brand diretamente, exceto quando indicado.

| Token | Light | Dark | Onde usar |
|---|---|---|---|
| `OnBackground` | `#0D253F` | `#FFFFFF` | Títulos de página, texto principal sobre o fundo da tela |
| `OnSurface` | `#666666` | `#B0BEC5` | Subtítulos, metadados (ano, gênero, duração), labels de navegação inativos |

### Brand

Cores da identidade visual do TMDB. Usadas em elementos interativos e de destaque — não como fundo de página.

| Token | Hex | Onde usar |
|---|---|---|
| `Primary` | `#0D253F` | Cor da TopBar no light mode quando houver contraste intencional; texto sobre fundo claro em elementos de marca |
| `Secondary` | `#01B4E4` | Links, ícones ativos na BottomBar, badges de categoria, bordas de destaque |
| `Tertiary` | `#90CEA1` | Tags de gênero, indicadores de disponibilidade, elementos decorativos suaves |
| `Teal` | `#1ED5A9` | Ponta inicial dos gradientes brand — raramente usado isolado |

### Gradientes

Extraídos diretamente do site do TMDB. Sempre aplicados da esquerda para a direita.

| Token | Valor | Onde usar |
|---|---|---|
| `GradientBrand` | `#1ED5A9` → `#01B4E4` | Botões de ação primária (ex: "Ver Trailer", "Adicionar à Lista"), logo do app, elementos hero de destaque |
| `GradientSegmented` | `#C0FECF` → `#1ED5A9` | Item selecionado no controle segmentado (ex: "Trailers / Imagens / Extras") |
| `GradientRating` | `#FDC170` → `#D93B63` | Barra de avaliação do usuário, score circular de popularidade |

### Overlay

Aplicado sobre imagens e posters para garantir legibilidade do texto em cima das capas.

| Token | Light | Dark | Onde usar |
|---|---|---|---|
| `Overlay` | `rgba(3, 37, 65, 0.60)` | `rgba(3, 37, 65, 0.75)` | Gradiente sobre posters na hero section, texto sobre imagens de backdrop, cards com título sobreposto à imagem |

---

## Navegação principal

**Top Bar (global — todas as telas principais):**

| Esquerda | Centro | Direita |
|---|---|---|
| Ícone de usuário (👤) | Logo TMDB | Ícone de busca (🔍) |

- Ícone de usuário: abre tela de Perfil/Configurações
- Ícone de busca: comportamento M3 — ao tocar, anima e expande o campo de busca sobre a top bar
- Configurações acessadas dentro do perfil — sem ícone dedicado na top bar

**5 abas (Bottom Navigation Bar — Android e iOS):**

| Aba | Ícone | Descrição |
|---|---|---|
| Home | home | Tela inicial, abre por padrão |
| Filmes | filme | Seções de filmes |
| Séries | tv | Seções de séries |
| Pessoas | pessoa | Lista de pessoas populares |
| Awards | troféu | Lista de premiações |

**Bottom Bar — visual:**
- Padrão nativo do MAUI como ponto de partida
- Melhorias visuais a avaliar durante o desenvolvimento

> **Busca:** não é mais uma aba. Acessada pelo ícone M3 na top bar. Ao ativar, anima e exibe o campo de busca com o mesmo comportamento e categorias de resultado que estava planejado para a aba.

---

## Internacionalização

- Todas as strings hardcoded no `.resx` — sem exceção
- Idiomas suportados: `pt-BR`, `en-US`, `es-ES`, `fr-FR`, `de-DE`
- Conteúdo da API (títulos, sinopses): parâmetro `language` passa o locale do dispositivo automaticamente
- Usuário pode sobrescrever o idioma nas **Configurações**

---

## Design

- Base de referência: **Material Design 3 (M3)** — especialmente para campos de texto (floating label), botões e padrões de interação
- Componentes MAUI customizados onde o nativo for visualmente inadequado
- Paleta de cores definida — ver seção **Paleta de cores** acima

### Elevação de cards

Cards sempre usam a cor `Surface` — nunca `Background` — criando profundidade visual pela diferença de tom entre o fundo da tela e o card.

- **Sem sombra** — a elevação é comunicada exclusivamente pela cor (Surface vs Background), sem `BoxShadow` ou `Shadow`
- **Light mode:** card `#F4F4F4` sobre fundo `#FFFFFF` — sutil, limpo
- **Dark mode:** card `#0D253F` sobre fundo `#031D33` — contraste de azul escuro, estilo cinema
- Não usar `Border` nos cards como substituto de elevação — borda só quando tiver função semântica (ex: card selecionado, estado de foco)

---

## Componentes identificados até agora

> Levantamento baseado no mapeamento de todas as telas. Novos componentes podem surgir durante a implementação.

### Visão geral — ContentView vs Style

> Overview inicial — pode ser refinado durante o desenvolvimento.

**ContentView (componentes com estrutura e binding próprios):**

| Componente | Onde é usado |
|---|---|
| Card de título | Listas horizontais e grid vertical em todas as abas |
| Card de trailer | Seção Últimos Trailers |
| Card de pessoa | Aba Pessoas, resultados de busca |
| Card de Collection | Categoria Collections na busca |
| Item de Companies | Categoria Companies na busca |
| Chip de Keyword | Categoria Keywords na busca |
| Seção expansível (expander) | Abas Filmes, Séries e Busca |
| Segmented control | Tendências (Hoje/Semana), Grátis para Assistir (Filmes/TV) |
| Bottom Sheet | Filtros de seção, seleção de fonte no Find by ID |
| Skeleton / shimmer | Estado de loading em todas as seções |
| Indicador de conectividade | Global — aparece quando offline |

**Styles/ControlTemplate (apenas aparência, sem estrutura própria):**

| Componente | Observação |
|---|---|
| Top Bar | Componente nativo do MAUI — customizamos o conteúdo |
| Banner de seção | Imagem de fundo + gradiente — provavelmente só style/layout |
| Welcome Banner | Card com imagem + gradiente + texto |
| Badge de contagem | Estilo aplicado sobre um elemento existente |

---

## Componentes compartilhados

### Card de título (padrão)
Usado em listas horizontais (Tendências, Populares, Grátis para Assistir, etc.) e em grid vertical (tela "ver mais").
- Sem container ou wrapper — imagem com bordas arredondadas direto sobre o fundo
- Imagem portrait com bordas arredondadas, ocupa a maior parte da altura do card
- Título abaixo da imagem, branco, truncado com `...`
- Linha abaixo do título: `★ nota` à esquerda + `ano` à direita
- Feedback de tap: container arredondado externo aparece ao pressionar, com ~2-3px de padding em torno de todo o item (imagem + texto) — invisível no estado normal

### Card de trailer
Usado em Últimos Trailers.
- Mesmo conteúdo do card de título
- Mais comprido (proporção próxima de 16:9)
- Ícone de play sobreposto à imagem

### Item de Companies
Usado na categoria Companies da busca.
- Lista vertical dentro do expander
- Cada item: nome à esquerda + logo/imagem à direita (quando disponível)
- Sem rating, sem ano
- **A investigar:** alguns itens aparecem sem logo — verificar se a API não retorna logo para esses casos ou se o formato da imagem é incompatível com o layout

### Card de Collection
Usado na categoria Collections da busca.
- Mesmo padrão do card de título — lista horizontal
- Imagem + nome abaixo truncado com `...`
- Sem rating, sem ano

### Chip de Keyword
Usado na categoria Keywords da busca.
- Não são cards — são chips/tags
- Pill com fundo colorido e bordas arredondadas
- Texto da keyword dentro do chip

### Card de pessoa
Usado na aba Pessoas e resultados de busca.
- Mesma estrutura do card de título — só muda a linha inferior
- Foto portrait com bordas arredondadas (mesmo formato do poster de filme/série)
- Nome abaixo da foto, branco
- Abaixo do nome: títulos em que participou, texto menor, truncado com `...`
- Feedback de tap: mesmo padrão do card de título

### Seção expansível (expander)
Usado nas abas Filmes e Séries para todas as seções exceto Tendências.
- Card com fundo levemente elevado, bordas arredondadas, largura quase total com margens laterais
- **Estado fechado:** título + chevron (↓) à esquerda — toda essa área é o tap target para expandir
- **Estado aberto:** título + chevron (↑) à esquerda + botão `all >` à direita + lista horizontal de cards abaixo
- Botão `todos >` (label internacionalizado) navega para a tela de lista completa da seção
- Animação de expand/collapse

### Bottom Sheet
Usado para seleção de opções em filtros de seção (Últimos Trailers, Os Mais Populares, etc.) e seleção de fonte no Find by ID.
- Substitui o dropdown customizado — padrão mobile mais natural
- Abre a partir da base da tela com drag handle no topo
- Lista de opções com nome à esquerda + ícone/logo à direita (quando aplicável)
- Descarta ao arrastar para baixo ou tocar fora

### Segmented control
Usado em seções com duas ou três opções fixas (ex: Hoje/Nesta semana, Filmes/TV).
- Controle customizado inline na seção

### Tela de lista completa (compartilhada)
Aberta ao tocar em `todos >` em qualquer seção expansível.
- Tela genérica — recebe título e fonte de dados como parâmetro, card muda conforme o tipo do item
- Possivelmente reutilizada também para pessoas (a confirmar)
- Top bar: seta de voltar (←) à esquerda + título da seção de origem no centro
- `CollectionView` com `GridItemsLayout` de 3 colunas
- Infinite scroll — paginação via API do TMDB

---

## Aba — Home

### Top Bar

| Esquerda | Centro | Direita |
|---|---|---|
| Ícone de usuário (👤) | Logo TMDB (ícone + texto empilhado) | Ícone de busca M3 (🔍) |

- Botão de usuário: formato squircle com fundo/borda sutil — nível de arredondamento a ajustar no design
- Logo centralizada: ícone de play + texto compacto empilhado
- Busca: ícone que ao tocar anima e expande campo de busca sobre a top bar (padrão M3 — estilo Google Drive/Gmail)
- Indicador de conectividade: a definir visualmente — provavelmente chip/banner que aparece sobre a UI apenas quando offline, sem ícone permanente na top bar

---

### Seção — Welcome Banner
- Card com bordas bem arredondadas, margens laterais pequenas
- Fundo: imagem dinâmica carregada de um dos endpoints da home + overlay de gradiente escuro/colorido
- Texto principal grande e bold (ex: "Bem-vindo.")
- Subtítulo menor abaixo
- Texto em branco sobre o gradiente

---

### Seção — Tendências

**Layout:**
- Banner de fundo ocupa **toda a seção** — título, segmented control e cards ficam sobrepostos a ele
- Banner: imagem do filme/série com maior avaliação na lista atual + gradiente escuro para garantir legibilidade
- Header sobreposto ao banner: `Tendências` (esquerda) + segmented control `Hoje | Esta semana` (direita) na mesma linha
  - Segmented control tem fundo escuro próprio (pill shape) para legibilidade sobre a imagem
  - Item selecionado exibe checkmark (✓) antes do texto
- Lista horizontal de cards sobreposta ao banner, abaixo do header

**Dados:**
- API: `trending/movie` e `trending/tv` — pessoas excluídas
- Alternância Hoje/Semana troca entre `time_window=day` e `time_window=week`
- Clique no item → navega para detalhe (filme ou série conforme `media_type`)

---

### Seção — Últimos Trailers

**Layout:**
- Sem card envolvendo a seção
- Background: imagem fixa de um dos trailers **ou** muda conforme scroll — **em aberto, testar na implementação**
- Header: `Últimos Trailers` (esquerda) + dropdown de categoria (direita)
- Lista horizontal de cards de trailer

**Filtros (dropdown customizado):**
- Populares
- Streaming
- Na TV
- Para Alugar
- Nos Cinemas

**Reprodução:**
- Clique no card → player in-app
- Fallback: abrir no YouTube caso player falhe

---

### Seção — Os Mais Populares

**Layout:**
- Envolto em card com bordas arredondadas
- Header: `Os Mais Populares` (esquerda) + dropdown de categoria (direita)
- Lista horizontal de cards de título (padrão)

**Filtros (dropdown customizado):**
- Streaming
- Na TV
- Para Alugar
- Nos Cinemas

**Dados:**
- Clique no item → detalhe

---

### Seção — Grátis para Assistir

**Layout:**
- Mesmo esquema de Os Mais Populares (envolto em card)
- Header: `Grátis para Assistir` (esquerda) + segmented control `Filmes | TV` (direita)
- Lista horizontal de cards de título (padrão)

**Dados:**
- Alternância Filmes/TV troca o endpoint consultado
- Clique no item → detalhe

---

### Seção — Líderes

**Layout:**
- Grid 2 colunas com os principais colaboradores da semana
- Por item:

```
[ foto ] Nome do usuário
         ████████████░░░  Total de Edições        (gradiente verde)
         ██████░░░░░░░░░  Edições Nesta Semana    (gradiente laranja → rosa)
```

- Foto do usuário: circular
- Duas barras de progresso com gradientes distintos
- Botão/link "Ver ranking completo" ao final da seção → navega para **Tela — Placar de Colaboradores**

---

## Aba — Filmes

### Seção — Tendências (fixa, sempre aberta)
- Idêntica à seção Tendências da Home
- Dados: somente filmes (`trending/movie`)

---

### Padrão das demais seções (expander)
Todas as seções abaixo seguem o mesmo padrão:
- **Fechado:** header com título + contagem + ícone de seta
- **Aberto:** lista horizontal de cards de título + botão "Mais" alinhado onde ficaria o dropdown
- Botão "Mais" → navega para **Tela de lista completa** da seção
- Clique no item → **Detalhe do filme**

### Seção — Populares
- API: `movie/popular`

### Seção — Em Cartaz
- API: `movie/now_playing`

### Seção — Próximas Estreias
- API: `movie/upcoming`

### Seção — Mais Bem Avaliados
- API: `movie/top_rated`

---

## Aba — Séries

Estrutura idêntica à aba de Filmes.

### Seção — Tendências (fixa, sempre aberta)
- Idêntica à seção Tendências da Home
- Dados: somente séries (`trending/tv`)

### Seção — Populares
- API: `tv/popular`

### Seção — Em Exibição Hoje
- API: `tv/airing_today`

### Seção — Na TV
- API: `tv/on_the_air`

### Seção — Mais Bem Avaliados
- API: `tv/top_rated`

---

## Aba — Pessoas

- Única seção: lista direta de populares
- `CollectionView` com `GridItemsLayout` de 3 colunas, cards de pessoa
- Infinite scroll — paginação via `person/popular`
- Clique → Detalhe da pessoa

---

## Aba — Awards

- Lista direta de premiações (mesmo padrão estrutural da aba Pessoas)
- `CollectionView` com `GridItemsLayout` — número de colunas a definir (avaliar 2 ou 3 conforme proporção dos logos)
- Infinite scroll — paginação via API do TMDB
- Filtros: Popular, Latest Ceremony (bottom sheet)
- Por item: logo da premiação + nome + data da última edição
- Clique → Detalhe da premiação (a mapear)

---

## Busca (via Top Bar M3)

Ativada pelo ícone 🔍 na top bar. Ao tocar:
- Animação M3: campo de busca expande sobre a top bar
- Comportamento e categorias de resultado idênticos ao que estava planejado para a aba Busca
- Sub-abas: `Buscar | Buscar por ID` (mesmo layout descrito abaixo)
- Fecha ao cancelar ou pressionar voltar

---

## Busca — Sub-abas

> Conteúdo exibido ao ativar a busca pela top bar M3. Não é mais uma aba do bottom navigation.

Sub-abas (segmented control no topo): `Buscar | Buscar por ID`

---

### Sub-aba — Buscar

**Sub-abas:** indicador de underline na aba ativa (não segmented control pill)

**Campo de busca:**
- Floating label M3: `Buscar filme, série...`
- Botão X (limpar) dentro do campo, à direita
- Botão `Buscar` fora do campo, à direita — bordas arredondadas, fundo colorido
- Botão desabilitado até o usuário digitar algo

**Resultados — expander (mesmo padrão de Filmes/Séries + badge de contagem):**
- Todos colapsados inicialmente
- **Estado fechado:** nome da categoria (esquerda) + badge circular colorido com contagem (ex: `275`) + chevron ↓ (direita)
- **Estado aberto:** lista horizontal com cards no padrão já definido + botão `todos >` (aparece somente quando há mais itens do que os exibidos na lista horizontal, ~20)
- Clique no item → detalhe correspondente

**Categorias retornadas:**
- Movies
- TV Shows
- People
- Companies
- Collections
- Keywords

---

### Sub-aba — Buscar por ID

- Campo de texto `Find by ID` + botão `Source ▼` + botão `Search` (desabilitado até preencher)
- Botão `Source` abre um **Bottom Sheet** com drag handle
  - Lista de fontes: nome à esquerda + ícone da plataforma à direita (squircle colorido)
  - Fontes: IMDb, The TVDB, Facebook, Twitter, Instagram, TikTok, YouTube, Wikidata...
- Resultados no mesmo padrão da busca convencional
- Categorias de resultado: Movies, TV Shows, TV Show seasons, TV Show episodes, People
- **A decidir:** avaliar se essa feature entra no escopo do app — funcionamento da API não está claro e o caso de uso é nichado. Revisar quando chegar na implementação da busca

---

## Tela — Placar de Colaboradores

Acessada via "Ver ranking completo" na seção Líderes da Home.

**Layout:**
- Gráfico de barras no topo: número de edições por dia no período selecionado
- Filtro de período: `7d | 1m | 6m` (alinhado à direita acima do gráfico)
- Abaixo do gráfico: título da semana atual (ex: "Principais colaboradores de Apr 08–14")
- Lista rankeada vertical com posição, avatar, nome de usuário, total de edições da semana, membro desde, média de edições por dia

**Top Bar:**
- Seta de voltar (←) à esquerda + "Placar de Colaboradores" no centro


