using System;
using System.Data.SQLite;
using System.IO;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Classe responsável pela configuração e inicialização do banco de dados SQLite
    /// </summary>
    public class DatabaseContext
    {
        private readonly string _connectionString;
        private readonly string _databasePath;

        public DatabaseContext()
        {
            _databasePath = Path.Combine(Directory.GetCurrentDirectory(), "apostas.db");
            _connectionString = $"Data Source={_databasePath};Version=3;";
            InitializeDatabase();
        }

        /// <summary>
        /// Obtém a string de conexão com o banco de dados
        /// </summary>
        public string GetConnectionString()
        {
            return _connectionString;
        }

        /// <summary>
        /// Inicializa o banco de dados criando as tabelas necessárias
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                using var connection = new SQLiteConnection(_connectionString);
                connection.Open();

                // Criar tabela de usuários
                var createUsuariosTable = @"
                    CREATE TABLE IF NOT EXISTS Usuarios (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL,
                        Email TEXT UNIQUE NOT NULL,
                        Saldo DECIMAL(10,2) NOT NULL DEFAULT 0,
                        DataCadastro DATETIME NOT NULL,
                        Ativo BOOLEAN NOT NULL DEFAULT 1,
                        NivelRisco INTEGER NOT NULL DEFAULT 0,
                        PontuacaoRisco INTEGER NOT NULL DEFAULT 0,
                        UltimaAvaliacao DATETIME NOT NULL,
                        RecebeAlertas BOOLEAN NOT NULL DEFAULT 1,
                        AceitaApoio BOOLEAN NOT NULL DEFAULT 0,
                        Telefone TEXT,
                        DataUltimaAposta DATETIME,
                        TotalApostasHoje INTEGER NOT NULL DEFAULT 0,
                        ValorApostadoHoje DECIMAL(10,2) NOT NULL DEFAULT 0,
                        DiasConsecutivosApostando INTEGER NOT NULL DEFAULT 0,
                        ConsentimentoAceito BOOLEAN NOT NULL DEFAULT 0,
                        DataConsentimento DATETIME
                    )";

                // Criar tabela de apostas
                var createApostasTable = @"
                    CREATE TABLE IF NOT EXISTS Apostas (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UsuarioId INTEGER NOT NULL,
                        TipoAposta TEXT NOT NULL,
                        Valor DECIMAL(10,2) NOT NULL,
                        Multiplicador DECIMAL(5,2) NOT NULL,
                        Status TEXT NOT NULL DEFAULT 'Pendente',
                        ValorGanho DECIMAL(10,2),
                        DataAposta DATETIME NOT NULL,
                        DataResultado DATETIME,
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                    )";

                // Criar tabela de histórico
                var createHistoricoTable = @"
                    CREATE TABLE IF NOT EXISTS Historico (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UsuarioId INTEGER NOT NULL,
                        TipoOperacao TEXT NOT NULL,
                        Valor DECIMAL(10,2) NOT NULL,
                        Descricao TEXT NOT NULL,
                        DataOperacao DATETIME NOT NULL,
                        SaldoAnterior DECIMAL(10,2) NOT NULL,
                        SaldoPosterior DECIMAL(10,2) NOT NULL,
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                    )";

                // Criar tabela de comportamentos de risco
                var createComportamentoRiscoTable = @"
                    CREATE TABLE IF NOT EXISTS ComportamentoRisco (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UsuarioId INTEGER NOT NULL,
                        Tipo INTEGER NOT NULL,
                        Descricao TEXT NOT NULL,
                        Severidade INTEGER NOT NULL,
                        DataDetectado DATETIME NOT NULL,
                        FoiNotificado BOOLEAN NOT NULL DEFAULT 0,
                        AcaoRecomendada TEXT NOT NULL,
                        AcaoExecutada BOOLEAN NOT NULL DEFAULT 0,
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                    )";

                // Criar tabela de intervenções
                var createIntervencaoTable = @"
                    CREATE TABLE IF NOT EXISTS Intervencao (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UsuarioId INTEGER NOT NULL,
                        Tipo INTEGER NOT NULL,
                        Titulo TEXT NOT NULL,
                        Mensagem TEXT NOT NULL,
                        AcaoRecomendada TEXT NOT NULL,
                        DataIntervencao DATETIME NOT NULL,
                        FoiVisualizada BOOLEAN NOT NULL DEFAULT 0,
                        FoiAceita BOOLEAN NOT NULL DEFAULT 0,
                        DataVisualizacao DATETIME,
                        DataAceitacao DATETIME,
                        Prioridade INTEGER NOT NULL DEFAULT 1,
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                    )";

                // Criar tabela de atividades alternativas
                var createAtividadeAlternativaTable = @"
                    CREATE TABLE IF NOT EXISTS AtividadeAlternativa (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Nome TEXT NOT NULL,
                        Descricao TEXT NOT NULL,
                        Categoria INTEGER NOT NULL,
                        DuracaoMinutos INTEGER NOT NULL,
                        CustoAproximado DECIMAL(10,2) NOT NULL DEFAULT 0,
                        Localizacao TEXT NOT NULL,
                        RequerEquipamento BOOLEAN NOT NULL DEFAULT 0,
                        EquipamentoNecessario TEXT,
                        NivelDificuldade INTEGER NOT NULL DEFAULT 1,
                        Beneficios TEXT NOT NULL,
                        Ativa BOOLEAN NOT NULL DEFAULT 1
                    )";

                // Criar tabela de relatórios comportamentais
                var createRelatorioComportamentalTable = @"
                    CREATE TABLE IF NOT EXISTS RelatorioComportamental (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UsuarioId INTEGER NOT NULL,
                        DataInicio DATETIME NOT NULL,
                        DataFim DATETIME NOT NULL,
                        TotalApostas INTEGER NOT NULL DEFAULT 0,
                        ValorTotalApostado DECIMAL(10,2) NOT NULL DEFAULT 0,
                        ValorTotalGanho DECIMAL(10,2) NOT NULL DEFAULT 0,
                        ValorTotalPerdido DECIMAL(10,2) NOT NULL DEFAULT 0,
                        DiasApostando INTEGER NOT NULL DEFAULT 0,
                        ApostasConsecutivas INTEGER NOT NULL DEFAULT 0,
                        ApostasNoturnas INTEGER NOT NULL DEFAULT 0,
                        MaiorAposta DECIMAL(10,2) NOT NULL DEFAULT 0,
                        MenorAposta DECIMAL(10,2) NOT NULL DEFAULT 0,
                        MediaApostas DECIMAL(10,2) NOT NULL DEFAULT 0,
                        PontuacaoRiscoInicial INTEGER NOT NULL DEFAULT 0,
                        PontuacaoRiscoFinal INTEGER NOT NULL DEFAULT 0,
                        NivelRiscoInicial INTEGER NOT NULL DEFAULT 0,
                        NivelRiscoFinal INTEGER NOT NULL DEFAULT 0,
                        TotalIntervencoes INTEGER NOT NULL DEFAULT 0,
                        IntervencoesAceitas INTEGER NOT NULL DEFAULT 0,
                        AnaliseComportamental TEXT NOT NULL,
                        Recomendacoes TEXT NOT NULL,
                        DataGeracao DATETIME NOT NULL,
                        FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                    )";

                using var command = new SQLiteCommand(createUsuariosTable, connection);
                command.ExecuteNonQuery();

                command.CommandText = createApostasTable;
                command.ExecuteNonQuery();

                command.CommandText = createHistoricoTable;
                command.ExecuteNonQuery();

                command.CommandText = createComportamentoRiscoTable;
                command.ExecuteNonQuery();

                command.CommandText = createIntervencaoTable;
                command.ExecuteNonQuery();

                command.CommandText = createAtividadeAlternativaTable;
                command.ExecuteNonQuery();

                command.CommandText = createRelatorioComportamentalTable;
                command.ExecuteNonQuery();

                Console.WriteLine("✅ Banco de dados inicializado com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao inicializar banco de dados: {ex.Message}");
                throw;
            }
        }
    }
}
