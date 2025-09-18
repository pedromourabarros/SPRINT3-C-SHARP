# 📋 Resumo do Projeto - Sistema de Detecção de Apostas Compulsivas

## 🎯 Objetivo do Projeto
Desenvolvimento de uma solução completa para detecção, inibição e tratamento de comportamentos compulsivos relacionados a apostas, como parte do **Challenge XP - Case 1**. O sistema utiliza algoritmos de análise comportamental e oferece intervenções personalizadas para ajudar usuários em situação de risco.

**Equipe Responsável:**
- RM550161 - Eduardo Osorio Filho
- RM550610 - Fabio Hideki Kamikihara  
- RM550260 - Pedro Moura Barros
- RM98896 - Rodrigo Fernandes dos Santos

---

## ✅ Requisitos Atendidos

### 1. CRUD Completo com Banco de Dados ✅
- **Banco**: SQLite configurado automaticamente
- **Tabelas**: Usuarios, Apostas, Historico
- **Operações**: Create, Read, Update, Delete
- **Relacionamentos**: Chaves estrangeiras implementadas
- **Validações**: Regras de negócio aplicadas

### 2. Manipulação de Arquivos ✅
- **Arquivos .txt**: Histórico de operações
- **Arquivos .json**: Usuários, apostas e relatórios
- **Backup**: Sistema completo de backup
- **Exportação**: Múltiplos formatos disponíveis

### 3. Interface Console ✅
- **Menu interativo**: Navegação intuitiva
- **Validação de entrada**: Dados consistentes
- **Feedback visual**: Mensagens claras
- **Tratamento de erros**: Exceções capturadas

### 4. Interface API (Swagger) ✅
- **Swagger UI**: Interface web interativa
- **Endpoints REST**: Usuários, apostas e relatórios
- **Testes diretos no navegador**: Try it out
- **Documentação automática** dos endpoints

### 5. Documentação Completa ✅
- **README.md**: Documentação principal
- **DEMONSTRACAO.md**: Exemplos de uso
- **INSTALACAO.md**: Guia de instalação
- **Comentários**: Código documentado

### 6. Diagramas UML/ER ✅
- **Diagrama de Classes**: Estrutura completa
- **Diagrama ER**: Modelo do banco
- **Diagrama de Sequência**: Fluxo de operações
- **Diagrama de Arquitetura**: Visão geral

### 7. Arquitetura em Camadas ✅
- **Models**: Entidades do domínio
- **Repository**: Acesso a dados
- **Services**: Regras de negócio
- **Controllers**: Endpoints da API
- **Middleware**: Tratamento de erros
- **Program**: Interface do usuário

### 8. Código Limpo ✅
- **Padrões**: Repository, Service, DTO
- **Separação**: Responsabilidades bem definidas
- **Nomenclatura**: Convenções C# seguidas
- **Comentários**: Código autoexplicativo

---

## 🏗️ Arquitetura Implementada

```
┌─────────────────────────────────────────┐
│           APRESENTAÇÃO                  │
│  ┌─────────────────────────────────────┐ │
│  │   Interface Console (Program.cs)    │ │
│  │   API Swagger (Controllers/)         │ │
│  └─────────────────────────────────────┘ │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│         REGRAS DE NEGÓCIO               │
│  ┌─────────────────────────────────────┐ │
│  │        Services                     │ │
│  │  • UsuarioService                   │ │
│  │  • ApostaService                    │ │
│  │  • HistoricoService                 │ │
│  │  • FileService                      │ │
│  └─────────────────────────────────────┘ │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│         ACESSO A DADOS                  │
│  ┌─────────────────────────────────────┐ │
│  │        Repositories                 │ │
│  │  • UsuarioRepository                │ │
│  │  • ApostaRepository                 │ │
│  │  • HistoricoRepository              │ │
│  │  • DatabaseContext                  │ │
│  └─────────────────────────────────────┘ │
└─────────────────────────────────────────┘
                    │
┌─────────────────────────────────────────┐
│           PERSISTÊNCIA                  │
│  ┌─────────────────────────────────────┐ │
│  │        SQLite Database              │ │
│  │  • Tabela Usuarios                  │ │
│  │  • Tabela Apostas                   │ │
│  │  • Tabela Historico                 │ │
│  └─────────────────────────────────────┘ │
│  ┌─────────────────────────────────────┐ │
│  │        Sistema de Arquivos          │ │
│  │  • Arquivos .txt                    │ │
│  │  • Arquivos .json                   │ │
│  │  • Backups                          │ │
│  └─────────────────────────────────────┘ │
└─────────────────────────────────────────┘
```

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia | Versão | Propósito |
|------------|--------|-----------|
| .NET | 8.0 | Framework principal |
| SQLite | 1.0.118 | Banco de dados |
| Newtonsoft.Json | 13.0.3 | Serialização JSON |
| Swashbuckle.AspNetCore | 6.5.0 | Documentação Swagger |
| C# | 12.0 | Linguagem de programação |

---

## 📁 Estrutura de Arquivos

```
ApostasCompulsivas/
├── 📁 Models/                 # Entidades
├── 📁 Repository/             # Acesso a dados
├── 📁 Services/               # Regras de negócio
├── 📁 Controllers/            # Endpoints da API
├── 📁 Middleware/              # Tratamento de erros
├── 📁 diagramas/               # Diagramas UML/ER e prints Swagger
├── 📁 Arquivos/                 # Backups (criada automaticamente)
├── apostas.db                   # Banco SQLite (criado automaticamente)
├── ApostasCompulsivas.csproj
├── Program.cs                   # Interface console
├── README.md
├── DEMONSTRACAO.md
├── INSTALACAO.md
└── RESUMO_PROJETO.md
```

---

## 🚀 Como Executar

### 1. Pré-requisitos
- .NET 8.0 SDK instalado
- Terminal/Command Prompt

### 2. Instalação
```bash
# Restaurar dependências
dotnet restore

# Compilar projeto
dotnet build

# Executar sistema em modo console
dotnet run

# Executar sistema em modo API
dotnet run -- --api
```

Depois acesse:
```
http://localhost:5000/swagger
```

---

## 📈 Métricas do Projeto

| Métrica | Valor |
|---------|-------|
| **Linhas de Código** | ~2.800 |
| **Classes** | 20+ |
| **Interfaces** | 10+ |
| **Métodos** | 90+ |
| **Arquivos** | 25+ |
| **Funcionalidades** | 30+ |

---

## 🎯 Critérios de Avaliação

### ✅ Funcionalidade (40 pontos)
- [x] CRUD completo implementado
- [x] Banco de dados funcional
- [x] Manipulação de arquivos
- [x] Interface console interativa
- [x] Interface API com Swagger
- [x] Relatórios e estatísticas

### ✅ Qualidade do Código (30 pontos)
- [x] Arquitetura em camadas
- [x] Padrões de projeto aplicados
- [x] Código limpo e documentado
- [x] Tratamento de erros
- [x] Validações de negócio

### ✅ Documentação (20 pontos)
- [x] README completo
- [x] Comentários no código
- [x] Diagramas UML/ER
- [x] Exemplos de uso
- [x] Guias de instalação

### ✅ Inovação (10 pontos)
- [x] Interface intuitiva
- [x] Sistema de backup robusto
- [x] Relatórios detalhados
- [x] Validações avançadas
- [x] Arquitetura escalável

---

## 🎉 Conclusão

O projeto **Sistema de Apostas Compulsivas** foi desenvolvido com sucesso, atendendo a todos os requisitos do Challenge XP e está pronto para avaliação.

**🚀 Pronto para Produção!**
