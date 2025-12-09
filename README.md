# FakeStoreDBAPI — Web API em .NET 8

## Visão geral
Este repositório contém uma API RESTful construída com ASP.NET Core (.NET 8) e Entity Framework Core. O projeto segue uma arquitetura em camadas (Controllers → Services → Data/Repository) e foi projetado para ser uma base robusta para aplicações que necessitam de um banco de dados relacional.

A API implementa seu próprio banco de dados para persistir os dados. A solução está configurada com injeção de dependência, logging com Serilog, mapeamento de objetos com AutoMapper e documentação de API com Swagger.

## Tecnologias e Bibliotecas Essenciais
- **.NET 8**: Plataforma de desenvolvimento principal.
- **Entity Framework Core 8**: ORM para interação com o banco de dados SQL Server. Utiliza o padrão de repositório através do `DbContext`.
- **AutoMapper**: Biblioteca para mapeamento automático entre DTOs (Data Transfer Objects) e modelos de domínio.
- **Swashbuckle.AspNetCore**: Geração de documentação OpenAPI (Swagger) para facilitar a exploração e o teste dos endpoints.
- **Serilog**: Biblioteca para logging estruturado, configurado para saídas no console e em arquivos.
- **ASP.NET Core**: Framework para construção da API, incluindo o sistema de injeção de dependência nativo.

## Estrutura do Projeto
A solução está contida em um único projeto executável, mas organizada em pastas que separam as responsabilidades:
- `FakeStoreDBAPI.Host`:
    - **Responsibilidade**: Ponto de entrada da aplicação. Configura e executa o host da API.
    - **`Controllers`**: Camada de apresentação. Recebe as requisições HTTP e orquestra as respostas.
    - **`Services`**: Camada de serviço. Contém a lógica de negócio e faz a ponte entre os controllers e a camada de dados.
    - **`Data`**: Camada de acesso a dados. Inclui o `DbContext` e as configurações do Entity Framework.
    - **`DTO`**: Objetos de transferência de dados, usados para modelar as entradas e saídas da API.
    - **`Mappers`**: Perfis de configuração do AutoMapper.
    - **`Migrations`**: Scripts de migração do banco de dados gerados pelo EF Core.
    - **`Middleware`**: Middlewares customizados, como o de tratamento global de exceções.

## Endpoints

### Endereços (Address)
- `GET /api/v1/Address`
    - Retorna todos os endereços cadastrados.
    - **Resposta 200**: Lista de `AddressDto`.

- `GET /api/v1/Address/{id}`
    - Retorna um endereço específico por ID.
    - **Resposta 200**: `AddressDto` correspondente ao ID.
    - **Resposta 404**: Se o endereço não for encontrado.

- `POST /api/v1/Address`
    - Cria um novo endereço.
    - **Body**: `CreateAddressDto`.
    - **Resposta 201**: Retorna o endereço recém-criado com a URL para acessá-lo no header `Location`.

- `PATCH /api/v1/Address/{id}`
    - Atualiza parcialmente um endereço existente.
    - **Body**: `UpdateAddressDto`.
    - **Resposta 204**: Indica que a atualização foi bem-sucedida, sem conteúdo no corpo da resposta.
    - **Resposta 404**: Se o endereço não for encontrado.

- `DELETE /api/v1/Address/{id}`
    - Faz o "soft delete" do registro apenas definindo ele como inativo (IsActive = false).
    - **Resposta 204**: Indica que "soft delete" foi bem-sucedido, sem conteúdo no corpo da resposta.
    - **Resposta 404**: Se o endereço não for encontrado.

## Configuração

### appsettings.json
Configurações principais da aplicação:
- **`ConnectionStrings.DefaultConnection`**: String de conexão para o banco de dados SQL Server.
- **`Logging`**: Configurações de nível de log para a aplicação.
- **`AllowedHosts`**: Hosts permitidos para acessar a aplicação.

### Perfis de execução (launchSettings.json)
- **HTTPS**: `https://localhost:5001` e `http://localhost:5000`
- **Ambiente Padrão**: `Development`, que habilita o Swagger e outras ferramentas de desenvolvimento.

## Arquitetura e Padrões de Projeto
- **Minimal APIs & `WebApplication`**: Utiliza o modelo de hospedagem simplificado do .NET 6+ para configurar o pipeline de requisições.
- **Injeção de Dependência (DI)**: Os serviços (`IAddressService`) e o `DbContext` são registrados no contêiner de DI com escopo de vida `Scoped`, seguindo as melhores práticas.
- **Padrão Repositório/Unit of Work**: O `FakeStoreDbContext` atua como uma implementação desses padrões, abstraindo o acesso direto ao banco e agrupando as operações.
- **Tratamento de Exceções Global**: Um middleware (`ExceptionHandlerMiddleware`) captura exceções não tratadas em toda a aplicação, gerando logs e retornando uma resposta de erro padronizada (HTTP 500), evitando o vazamento de detalhes de implementação. Exceções específicas, como `NotFoundException`, são tratadas nos serviços para retornar os códigos de status corretos (HTTP 404).
- **Migrations do EF Core**: A estrutura do banco de dados é gerenciada via código através de migrations, garantindo versionamento e reprodutibilidade do schema.
- **Logging**: Os logs são registrados em uma pasta própria chamada `logs` dentro do diretório base da aplicação.

## Como Executar e Desenvolver
1.  **Configurar o Banco de Dados**:
    - Certifique-se de ter um servidor SQL Server (ou LocalDB) em execução.
    - Atualize a `ConnectionStrings.DefaultConnection` no `appsettings.Development.json` com os dados do seu ambiente.

2.  **Aplicar as Migrations**:
    - Abra um terminal na pasta do projeto (`FakeStoreDBAPI.Host`).
    - Execute o comando a seguir para criar o banco de dados e aplicar o schema inicial:
      ```sh
      dotnet ef database update
      ```

3.  **Executar a Aplicação**:
    - Pressione `F5` no Visual Studio ou execute o comando no terminal:
      ```sh
      dotnet run
      ```

4.  **Testar a API**:
    - Navegue para a URL do Swagger (ex: `https://localhost:5001/swagger`) para visualizar e interagir com os endpoints.
