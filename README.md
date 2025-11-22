# EngageGov Content MCP Server

![.NET](https://img.shields.io/badge/.NET-9.0-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![License](https://img.shields.io/badge/license-MIT-green)

Servidor MCP (Model Context Protocol) desenvolvido em .NET com foco em segurança, boas práticas e arquitetura limpa. Este projeto demonstra um servidor moderno seguindo os princípios SOLID e Clean Code.

## 📋 Características

- **Arquitetura Limpa (Clean Architecture)**: Separação clara de responsabilidades em camadas
- **Princípios SOLID**: Código bem estruturado e manutenível
- **Segurança**: Implementação de múltiplas camadas de segurança
- **Docker**: Imagem otimizada usando Alpine Linux
- **Logging**: Configuração robusta com Serilog
- **Testes**: Cobertura de testes unitários com xUnit
- **API RESTful**: Endpoints bem documentados
- **Health Checks**: Monitoramento de saúde da aplicação

## 🏗️ Arquitetura

O projeto segue a arquitetura limpa (Clean Architecture) com separação em camadas:

```
EngageGovContentMcp/
├── src/
│   ├── EngageGovContentMcp.Domain/         # Entidades de domínio e lógica de negócio
│   │   ├── Common/                         # Classes base compartilhadas
│   │   ├── Entities/                       # Entidades do domínio
│   │   └── Exceptions/                     # Exceções do domínio
│   │
│   ├── EngageGovContentMcp.Application/    # Casos de uso e interfaces
│   │   ├── DTOs/                           # Data Transfer Objects
│   │   ├── Interfaces/                     # Contratos de serviços
│   │   └── Services/                       # Implementações de serviços
│   │
│   ├── EngageGovContentMcp.Infrastructure/ # Implementações de infraestrutura
│   │   ├── Data/                           # Contexto do banco de dados
│   │   └── Repositories/                   # Implementações de repositórios
│   │
│   └── EngageGovContentMcp.Server/         # Camada de apresentação (API)
│       ├── Controllers/                    # Controllers da API
│       └── Middleware/                     # Middlewares customizados
│
└── tests/
    └── EngageGovContentMcp.UnitTests/      # Testes unitários
```

### Princípios Aplicados

#### SOLID
- **S**ingle Responsibility: Cada classe tem uma única responsabilidade
- **O**pen/Closed: Aberto para extensão, fechado para modificação
- **L**iskov Substitution: Substituição de tipos base por derivados
- **I**nterface Segregation: Interfaces específicas por cliente
- **D**ependency Inversion: Dependência de abstrações, não implementações

#### Clean Code
- Nomes descritivos e significativos
- Funções pequenas e focadas
- Comentários apenas quando necessário
- Tratamento adequado de erros
- Formatação consistente

## 🔒 Recursos de Segurança

### Headers de Segurança
- `X-Content-Type-Options: nosniff`
- `X-Frame-Options: DENY`
- `X-XSS-Protection: 1; mode=block`
- `Referrer-Policy: no-referrer`
- `Content-Security-Policy`

### Práticas Implementadas
- Usuário não-root no container Docker
- Validação de entrada de dados
- Tratamento centralizado de exceções
- HTTPS redirection
- Health checks configurados
- Logging estruturado
- CORS configurável

## 🚀 Como Executar

### Pré-requisitos
- .NET 9.0 SDK
- Docker (opcional)
- Docker Compose (opcional)

### Executar Localmente

```bash
# Clone o repositório
git clone https://github.com/lucas-explica/engage-gov-content-mcp.git
cd engage-gov-content-mcp

# Restaurar dependências
dotnet restore

# Compilar o projeto
dotnet build

# Executar os testes
dotnet test

# Executar o servidor
cd src/EngageGovContentMcp.Server
dotnet run
```

O servidor estará disponível em `http://localhost:5000` ou `https://localhost:5001`.

### Executar com Docker

```bash
# Build da imagem
docker build -t engagegovcontent-mcp .

# Executar o container
docker run -p 8080:8080 --name engagegovcontent-mcp engagegovcontent-mcp
```

### Executar com Docker Compose

```bash
# Iniciar os serviços
docker-compose up -d

# Verificar logs
docker-compose logs -f

# Parar os serviços
docker-compose down
```

O servidor estará disponível em `http://localhost:8080`.

## 📚 API Endpoints

### Content Items

#### Listar todos os itens
```http
GET /api/contentitems
```

#### Obter item por ID
```http
GET /api/contentitems/{id}
```

#### Listar itens publicados
```http
GET /api/contentitems/published
```

#### Listar por categoria
```http
GET /api/contentitems/category/{category}
```

#### Criar novo item
```http
POST /api/contentitems
Content-Type: application/json

{
  "title": "Título do Conteúdo",
  "description": "Descrição do conteúdo",
  "content": "Conteúdo completo",
  "category": "Categoria"
}
```

#### Atualizar item
```http
PUT /api/contentitems/{id}
Content-Type: application/json

{
  "title": "Novo Título",
  "description": "Nova descrição",
  "content": "Novo conteúdo",
  "category": "Nova categoria"
}
```

#### Deletar item
```http
DELETE /api/contentitems/{id}
```

#### Publicar item
```http
POST /api/contentitems/{id}/publish
```

#### Despublicar item
```http
POST /api/contentitems/{id}/unpublish
```

### Health Check
```http
GET /health
```

### Informações do Serviço
```http
GET /
```

## 🧪 Testes

O projeto inclui testes unitários utilizando xUnit. Para executar os testes:

```bash
# Executar todos os testes
dotnet test

# Executar com cobertura de código
dotnet test /p:CollectCoverage=true
```

## 📊 Logging

O projeto utiliza Serilog para logging estruturado. Os logs são gravados em:
- Console (desenvolvimento)
- Arquivos rotativos em `logs/` (produção)

Configuração em `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information"
    }
  }
}
```

## 🔧 Configuração

### Variáveis de Ambiente

- `ASPNETCORE_ENVIRONMENT`: Define o ambiente (Development, Staging, Production)
- `ASPNETCORE_URLS`: URLs de binding do servidor

### appsettings.json

```json
{
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000"]
  },
  "Security": {
    "EnableRateLimiting": true,
    "RateLimitRequests": 100,
    "RateLimitPeriodSeconds": 60
  }
}
```

## 🐳 Detalhes do Docker

### Imagem Base
- **Build**: `mcr.microsoft.com/dotnet/sdk:9.0-alpine`
- **Runtime**: `mcr.microsoft.com/dotnet/aspnet:9.0-alpine`

### Otimizações
- Multi-stage build para reduzir tamanho da imagem
- Usuário não-root para segurança
- Health check integrado
- Imagem Alpine Linux (leve)

### Tamanho Aproximado
- Imagem final: ~100MB

## 🤝 Contribuindo

Contribuições são bem-vindas! Por favor:

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📝 Licença

Este projeto está licenciado sob a Licença MIT - veja o arquivo [LICENSE](LICENSE) para detalhes.

## 👨‍💻 Autor

**Lucas de Lima**

## 📞 Suporte

Para suporte e dúvidas, abra uma issue no repositório do GitHub.

---

**Nota**: Este projeto é um servidor MCP de demonstração que implementa as melhores práticas de desenvolvimento .NET, incluindo Clean Architecture, SOLID, segurança e Docker.

