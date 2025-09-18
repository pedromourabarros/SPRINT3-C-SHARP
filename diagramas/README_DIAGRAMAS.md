# 📊 Diagramas do Sistema de Detecção de Apostas Compulsivas

Este diretório contém os diagramas UML e ER do sistema, desenvolvidos em PlantUML para documentar a arquitetura e funcionamento da solução.

**Desenvolvido por:**
- RM550161 - Eduardo Osorio Filho
- RM550610 - Fabio Hideki Kamikihara  
- RM550260 - Pedro Moura Barros
- RM98896 - Rodrigo Fernandes dos Santos

## 📁 Arquivos Disponíveis

### 1. Diagrama de Classes (`diagrama_classes.puml`)
- **Descrição**: Mostra a estrutura completa das classes do sistema
- **Inclui**: Models, Repositories, Services e suas interfaces
- **Relacionamentos**: Herança, implementação e dependências

### 2. Diagrama ER (`diagrama_er.puml`)
- **Descrição**: Modelo entidade-relacionamento do banco de dados
- **Inclui**: Tabelas Usuarios, Apostas e Historico
- **Relacionamentos**: Chaves primárias, estrangeiras e cardinalidades

### 3. Diagrama de Sequência (`diagrama_sequencia.puml`)
- **Descrição**: Fluxo de execução para realizar uma aposta
- **Inclui**: Interação entre todas as camadas do sistema
- **Cenários**: Sucesso, validações e tratamento de erros

### 4. Diagrama de Arquitetura (`diagrama_arquitetura.puml`)
- **Descrição**: Visão geral da arquitetura em camadas
- **Inclui**: Apresentação, Negócio, Acesso a Dados e Persistência
- **Componentes**: Serviços, repositórios e sistemas de arquivo

### 5. Diagrama de Fluxo Geral (`diagrama_fluxo.png`)
- **Descrição**: Mostra o fluxo geral de informação do sistema
- **Inclui**: Todas as camadas (console, serviços, banco de dados e arquivos)
- **Formato**: Imagem PNG criada no draw.io

## 🛠️ Como Visualizar os Diagramas

### Opção 1: PlantUML Online
1. Acesse [PlantUML Online Server](http://www.plantuml.com/plantuml/uml/)
2. Copie o conteúdo de qualquer arquivo `.puml`
3. Cole no editor online
4. Clique em "Submit" para gerar o diagrama

### Opção 2: Extensão VS Code
1. Instale a extensão "PlantUML" no VS Code
2. Abra qualquer arquivo `.puml`
3. Use `Ctrl+Shift+P` e digite "PlantUML: Preview Current Diagram"

### Opção 3: Plugin IntelliJ/WebStorm
1. Instale o plugin "PlantUML integration"
2. Abra qualquer arquivo `.puml`
3. Use `Alt+D` para visualizar o diagrama

### Opção 4: Aplicação Desktop
1. Baixe o [PlantUML JAR](https://plantuml.com/download)
2. Execute: `java -jar plantuml.jar diagrama_classes.puml`
3. O diagrama será gerado em PNG/SVG

## 📋 Resumo dos Diagramas

### Diagrama de Classes
```
Models (3 classes)
├── Usuario
├── Aposta
└── Historico

Repository (6 classes)
├── Interfaces (3)
└── Implementações (3)

Services (8 classes)
├── Interfaces (4)
└── Implementações (4)

Program (1 classe)
└── Interface Console
```

### Diagrama ER
```
Usuarios (1:N) Apostas
Usuarios (1:N) Historico

Tabelas: 3
Relacionamentos: 2
Chaves: 3 PK, 2 FK
```

### Diagrama de Sequência
```
Fluxo: Realizar Aposta
Ator: Usuário
Participantes: 7
Cenários: 3 (sucesso, validação, erro)
```

### Diagrama de Arquitetura
```
Camadas: 4
├── Apresentação
├── Regras de Negócio
├── Acesso a Dados
└── Persistência

Componentes: 12
Sistemas: 2 (SQLite + Arquivos)
```

## 🎯 Objetivos dos Diagramas

### Para Desenvolvedores
- Entender a estrutura do código
- Identificar dependências entre classes
- Facilitar manutenção e evolução

### Para Avaliadores
- Demonstrar conhecimento de padrões de projeto
- Mostrar arquitetura bem estruturada
- Validar separação de responsabilidades

### Para Documentação
- Servir como referência técnica
- Facilitar onboarding de novos desenvolvedores
- Documentar decisões arquiteturais

## 🔧 Personalização

Os diagramas podem ser personalizados editando os arquivos `.puml`:

- **Cores**: Modifique `skinparam` para alterar cores
- **Tema**: Altere `!theme` para diferentes estilos
- **Layout**: Ajuste `skinparam linetype` para diferentes layouts
- **Conteúdo**: Adicione/remova elementos conforme necessário

## 📝 Notas Importantes

- Todos os diagramas seguem as convenções UML padrão
- Os relacionamentos são baseados no código real
- As cardinalidades refletem o banco de dados SQLite
- Os fluxos de sequência representam cenários reais do sistema

---

**💡 Dica**: Para uma melhor visualização, recomenda-se usar o PlantUML Online Server ou a extensão do VS Code, pois oferecem renderização em tempo real e alta qualidade.
