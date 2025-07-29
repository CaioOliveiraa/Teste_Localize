
# Cadastro de Empresas

  

Este repositório contém uma API REST desenvolvida em **ASP.NET 8** para cadastro e listagem de empresas. Os usuários podem registrar-se, realizar login e, a partir de um token JWT, consultar ou cadastrar empresas utilizando o CNPJ. Os dados das empresas são obtidos da API pública da ReceitaWS.

## Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [EF Core CLI](https://learn.microsoft.com/ef/core/cli/dotnet)
- [Node.js](https://nodejs.org/) 18 ou superior para executar o frontend

## Funcionalidades

  

-  **Autenticação com JWT** – Endpoints para registrar (`/auth/registrar`) e autenticar (`/auth/login`) usuários.

-  **Cadastro de empresas** – Registro de novas empresas via CNPJ e listagem das empresas do usuário autenticado (`/empresas`).

-  **Integração com ReceitaWS** – Busca automática de dados da empresa a partir do CNPJ informado (via [ReceitaWS](https://www.receitaws.com.br/)).

-  **Persistência com SQLite** – Banco de dados simples configurado via Entity Framework Core.

-  **Testes unitários** – Projeto de testes utilizando xUnit, Moq e FluentAssertions.

  

## Configuração

  

1.  **Clonar o repositório**

```bash

git clone <este-repositorio>

cd Teste_Localize

```

2.  **Ajustar `appsettings.json`** (caso deseje alterar a string de conexão ou as chaves do JWT):

```json

{

"ConnectionStrings": {

"DefaultConnection": "Data Source=cadastroempresas.db"

},

"Jwt": {

"Key": "<SUA_CHAVE_SECRETA>",

"Issuer": "<SEU_ISSUER>",

"Audience": "<SEU_AUDIENCE>"

}

}

```

3.  **Aplicar migrações e criar o banco de dados**

```bash

dotnet ef database update

```

4.  **Executar a aplicação**

```bash

dotnet run --project CadastroEmpresas

```

  

A API estará disponível por padrão em `https://localhost:5001` (ou porta definida pelo ASP.NET). Em desenvolvimento, o Swagger pode ser acessado em `/swagger`.


## Executando o frontend (Next.js)

1. Acesse o diretório `cadastro-empresas-web`:

```bash
cd cadastro-empresas-web
```

2. Instale as dependências:

```bash
npm install
```

3. Crie um arquivo `.env.local` definindo o endereço da API:

```bash
NEXT_PUBLIC_API_URL=http://localhost:5114
```

4. Inicie a aplicação web:

```bash
npm run dev
```

O site estará disponível em `http://localhost:3000`.

## Executando os testes

  

Para rodar a suíte de testes unitários:

  

```bash

cd  CadastroEmpresas.Tests

dotnet  test

```

  

## Estrutura do projeto

  

-  `CadastroEmpresas/` – Código da API principal.

-  `CadastroEmpresas.Tests/` – Projeto de testes automatizados.

-  `cadastro-empresas-web/` – Aplicação web construída com Next.js.

-  `CadastroEmpresas.sln` – Solução com os dois projetos.

  

## Contribuição

  

Contribuições são bem‑vindas! Abra issues ou pull requests com melhorias ou correções.

  

## Licença

  

Este projeto é disponibilizado sem uma licença específica.
