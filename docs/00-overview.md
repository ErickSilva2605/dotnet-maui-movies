# MauiMovies — Visão Geral do Projeto

## Objetivo

Aplicativo mobile desenvolvido com **.NET MAUI** que consome a API do [The Movie Database (TMDB)](https://www.themoviedb.org/), com o objetivo de reproduzir o máximo de funcionalidades do site oficial.

O projeto tem dois propósitos complementares:

1. **Portfólio enterprise** — demonstrar domínio de arquitetura de software, boas práticas, testes e UI/UX em nível profissional
2. **Laboratório técnico** — praticar padrões e estratégias que geralmente ficam de lado no desenvolvimento mobile

---

## Funcionalidades Planejadas

- Listagem de filmes, séries e pessoas (trending, populares, mais bem avaliados)
- Tela de detalhes completa (elenco, trailers, avaliações, similares)
- Busca multi-categoria (filmes, séries, pessoas)
- Suporte offline com dados em cache local
- Autenticação TMDB (favoritos, watchlist, avaliações)
- Temas claro e escuro

---

## Stack Tecnológica

| Categoria | Tecnologia |
|---|---|
| Framework | .NET 9 / .NET MAUI |
| MVVM | CommunityToolkit.Mvvm |
| Persistência local | EF Core + SQLite |
| Serialização JSON | System.Text.Json |
| Testes unitários | xUnit + Moq |
| Testes de UI | Appium |
| Target platforms | Android, iOS, macOS, Windows |

---

## Plataformas Suportadas

- Android
- iOS
- macOS (Mac Catalyst)
- Windows

---

## Documentação

| Documento | Descrição |
|---|---|
| [01-architecture.md](01-architecture.md) | Decisões arquiteturais, estrutura de projetos e responsabilidades |
| [02-data-flow.md](02-data-flow.md) | Representações de dados, mapeamentos e fluxo entre camadas |
| [03-testing.md](03-testing.md) | Estratégia de testes, cobertura e ferramentas |
