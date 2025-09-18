# 🛠️ Guia de Instalação - Sistema de Detecção de Apostas Compulsivas

## 📋 Requisitos do Sistema

### Software Necessário
- **.NET 8.0 SDK** ou versão superior
- **Visual Studio 2022** ou **VS Code** (recomendado)
- **Git** (para clonagem do repositório)

### Sistemas Suportados
- Windows 10/11
- Linux (Ubuntu 20.04+)
- macOS (10.15+)

**Equipe de Desenvolvimento:**
- RM550161 - Eduardo Osorio Filho
- RM550610 - Fabio Hideki Kamikihara  
- RM550260 - Pedro Moura Barros
- RM98896 - Rodrigo Fernandes dos Santos

---

## 🚀 Instalação Passo a Passo

### 1. Verificar Pré-requisitos

#### Verificar .NET SDK
```bash
dotnet --version
```
**Saída esperada**: `8.0.x` ou superior

#### Se não tiver o .NET instalado:
- **Windows**: Baixe em [dotnet.microsoft.com](https://dotnet.microsoft.com/download)
- **Linux**: 
  ```bash
  wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb
  sudo dpkg -i packages-microsoft-prod.deb
  sudo apt-get update
  sudo apt-get install -y dotnet-sdk-8.0
  ```
- **macOS**: 
  ```bash
  brew install --cask dotnet
  ```

---

### 2. Baixar o Projeto

#### Opção A: Clonar Repositório
```bash
git clone <url-do-repositorio>
cd ApostasCompulsivas
```

#### Opção B: Download ZIP
1. Baixe o arquivo ZIP do projeto
2. Extraia em uma pasta
3. Abra o terminal na pasta extraída

---

### 3. Restaurar Dependências

```bash
dotnet restore
```

**Saída esperada**:
```
Restauração concluída (2,9s)
```

---

### 4. Compilar o Projeto

```bash
dotnet build
```

**Saída esperada**:
```
ApostasCompulsivas êxito (0,6s) → bin\Debug\net8.0\ApostasCompulsivas.dll
Construir êxito em 1,3s
```

---

### 5. Executar o Sistema

```bash
# Executar em modo console
dotnet run

# Executar em modo API (Swagger)
dotnet run -- --api
```

**Acesse no navegador:**
```
http://localhost:5000/swagger
```

**Saída esperada (modo console):**
```
🚀 Inicializando sistema de apostas...
✅ Banco de dados inicializado com sucesso!
✅ Sistema inicializado com sucesso!

🎰 ================================================
    SISTEMA DE APOSTAS COMPULSIVAS - CHALLENGE XP
==================================================

1. 👤 Gerenciar Usuários
2. 🎲 Gerenciar Apostas
3. 📊 Relatórios e Histórico
4. 💾 Backup e Restauração
5. ⚙️  Configurações
0. 🚪 Sair

Escolha uma opção:
```

---

## 🔧 Configuração Avançada

### Variáveis de Ambiente (Opcional)

#### Windows
```cmd
set APOSTAS_DB_PATH=C:\MeusProjetos\ApostasCompulsivas\apostas.db
set APOSTAS_FILES_PATH=C:\MeusProjetos\ApostasCompulsivas\Arquivos
```

#### Linux/macOS
```bash
export APOSTAS_DB_PATH=/home/usuario/ApostasCompulsivas/apostas.db
export APOSTAS_FILES_PATH=/home/usuario/ApostasCompulsivas/Arquivos
```

### Configuração do Banco de Dados

O sistema usa SQLite por padrão. Para usar outro banco:

1. Edite `Repository/DatabaseContext.cs`
2. Altere a string de conexão
3. Instale o driver apropriado

### Configuração de Arquivos

Por padrão, os arquivos são salvos em:
- **Banco**: `apostas.db` (pasta do projeto)
- **Backups**: `Arquivos/` (pasta do projeto)

---

## 🐛 Solução de Problemas

### Erro: "dotnet não é reconhecido"
**Solução**: Instale o .NET SDK e reinicie o terminal

### Erro: "Falha na restauração de pacotes"
**Solução**:
```bash
dotnet nuget locals all --clear
dotnet restore
```

### Erro: "Falha na compilação"
**Solução**:
```bash
dotnet clean
dotnet restore
dotnet build
```

### Erro: "Banco de dados não encontrado"
**Solução**: O banco é criado automaticamente na primeira execução

### Erro: "Pasta Arquivos não encontrada"
**Solução**: A pasta é criada automaticamente na primeira execução

---

## 📁 Estrutura de Arquivos Após Instalação

```
ApostasCompulsivas/
├── 📁 bin/                    # Arquivos compilados
├── 📁 obj/                    # Arquivos temporários
├── 📁 Models/                 # Entidades do domínio
├── 📁 Repository/             # Acesso a dados
├── 📁 Services/               # Regras de negócio
├── 📁 Controllers/             # API REST com Swagger
├── 📁 Middleware/               # Tratamento global de erros
├── 📁 Arquivos/               # Backups e relatórios
├── 📁 diagramas/              # Diagramas UML/ER e prints do Swagger
├── apostas.db                 # Banco SQLite (criado automaticamente)
├── ApostasCompulsivas.csproj  # Arquivo do projeto
├── Program.cs                 # Programa principal
├── README.md                  # Documentação principal
├── DEMONSTRACAO.md             # Guia de demonstração
├── INSTALACAO.md               # Este arquivo
└── RESUMO_PROJETO.md
```

---

## 🚀 Execução em Diferentes Ambientes

### Desenvolvimento
```bash
dotnet run
```

### Produção
```bash
dotnet publish -c Release
cd bin/Release/net8.0/publish
dotnet ApostasCompulsivas.dll
```

---

## 📊 Verificação da Instalação

### Teste Básico
1. Execute o sistema: `dotnet run`
2. Cadastre um usuário
3. Realize uma aposta
4. Verifique se os arquivos foram criados

### Teste Completo
1. Execute todos os menus
2. Teste backup e restauração
3. Verifique relatórios
4. Confirme persistência dos dados

---

## ✅ Checklist de Instalação

- [ ] .NET 8.0 SDK instalado
- [ ] Projeto baixado/clonado
- [ ] Dependências restauradas
- [ ] Projeto compilado com sucesso
- [ ] Sistema executado sem erros
- [ ] Banco de dados criado
- [ ] Pasta Arquivos criada
- [ ] Menu principal exibido
- [ ] Funcionalidades testadas

---

## 🎉 Instalação Concluída!

Se todos os itens do checklist foram marcados, o sistema está pronto para uso!

**Próximos passos**:
1. Leia o `README.md` para entender o sistema
2. Veja o `DEMONSTRACAO.md` para exemplos de uso
3. Explore as funcionalidades disponíveis

**💡 Dica**: Mantenha sempre uma cópia de backup do banco de dados (`apostas.db`) em local seguro!
