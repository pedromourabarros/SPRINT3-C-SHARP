using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Implementação do repositório de histórico com SQLite
    /// </summary>
    public class HistoricoRepository : IHistoricoRepository
    {
        private readonly string _connectionString;

        public HistoricoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Historico> CreateAsync(Historico historico)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Historico (UsuarioId, TipoOperacao, Valor, Descricao, DataOperacao, SaldoAnterior, SaldoPosterior)
                VALUES (@UsuarioId, @TipoOperacao, @Valor, @Descricao, @DataOperacao, @SaldoAnterior, @SaldoPosterior);
                SELECT last_insert_rowid();";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@UsuarioId", DbType.Int32) { Value = historico.UsuarioId });
            command.Parameters.Add(new SQLiteParameter("@TipoOperacao", DbType.String) { Value = historico.TipoOperacao });
            command.Parameters.Add(new SQLiteParameter("@Valor", DbType.Decimal) { Value = historico.Valor });
            command.Parameters.Add(new SQLiteParameter("@Descricao", DbType.String) { Value = historico.Descricao });
            command.Parameters.Add(new SQLiteParameter("@DataOperacao", DbType.DateTime) { Value = historico.DataOperacao });
            command.Parameters.Add(new SQLiteParameter("@SaldoAnterior", DbType.Decimal) { Value = historico.SaldoAnterior });
            command.Parameters.Add(new SQLiteParameter("@SaldoPosterior", DbType.Decimal) { Value = historico.SaldoPosterior });

            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            historico.Id = id;

            return historico;
        }

        public async Task<List<Historico>> GetByUsuarioIdAsync(int usuarioId)
        {
            var historicos = new List<Historico>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Historico WHERE UsuarioId = @UsuarioId ORDER BY DataOperacao DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@UsuarioId", DbType.Int32) { Value = usuarioId });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                historicos.Add(MapHistorico(reader));
            }

            return historicos;
        }

        public async Task<List<Historico>> GetAllAsync()
        {
            var historicos = new List<Historico>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Historico ORDER BY DataOperacao DESC";
            using var command = new SQLiteCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                historicos.Add(MapHistorico(reader));
            }

            return historicos;
        }

        public async Task<List<Historico>> GetByTipoOperacaoAsync(string tipoOperacao)
        {
            var historicos = new List<Historico>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Historico WHERE TipoOperacao = @TipoOperacao ORDER BY DataOperacao DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@TipoOperacao", DbType.String) { Value = tipoOperacao });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                historicos.Add(MapHistorico(reader));
            }

            return historicos;
        }

        public async Task<List<Historico>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim)
        {
            var historicos = new List<Historico>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Historico WHERE DataOperacao BETWEEN @DataInicio AND @DataFim ORDER BY DataOperacao DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@DataInicio", DbType.DateTime) { Value = dataInicio });
            command.Parameters.Add(new SQLiteParameter("@DataFim", DbType.DateTime) { Value = dataFim });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                historicos.Add(MapHistorico(reader));
            }

            return historicos;
        }

        private Historico MapHistorico(System.Data.Common.DbDataReader reader)
        {
            return new Historico
            {
                Id = Convert.ToInt32(reader["Id"]),
                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                TipoOperacao = Convert.ToString(reader["TipoOperacao"]) ?? string.Empty,
                Valor = Convert.ToDecimal(reader["Valor"]),
                Descricao = Convert.ToString(reader["Descricao"]) ?? string.Empty,
                DataOperacao = Convert.ToDateTime(reader["DataOperacao"]),
                SaldoAnterior = Convert.ToDecimal(reader["SaldoAnterior"]),
                SaldoPosterior = Convert.ToDecimal(reader["SaldoPosterior"])
            };
        }
    }
}
