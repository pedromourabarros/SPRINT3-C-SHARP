using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Implementação do repositório de apostas com SQLite
    /// </summary>
    public class ApostaRepository : IApostaRepository
    {
        private readonly string _connectionString;

        public ApostaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Aposta> CreateAsync(Aposta aposta)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Apostas (UsuarioId, TipoAposta, Valor, Multiplicador, Status, ValorGanho, DataAposta, DataResultado)
                VALUES (@UsuarioId, @TipoAposta, @Valor, @Multiplicador, @Status, @ValorGanho, @DataAposta, @DataResultado);
                SELECT last_insert_rowid();";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@UsuarioId", DbType.Int32) { Value = aposta.UsuarioId });
            command.Parameters.Add(new SQLiteParameter("@TipoAposta", DbType.String) { Value = aposta.TipoAposta });
            command.Parameters.Add(new SQLiteParameter("@Valor", DbType.Decimal) { Value = aposta.Valor });
            command.Parameters.Add(new SQLiteParameter("@Multiplicador", DbType.Decimal) { Value = aposta.Multiplicador });
            command.Parameters.Add(new SQLiteParameter("@Status", DbType.String) { Value = aposta.Status });
            command.Parameters.Add(new SQLiteParameter("@ValorGanho", DbType.Decimal) { Value = aposta.ValorGanho.HasValue ? (object)aposta.ValorGanho.Value : DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@DataAposta", DbType.DateTime) { Value = aposta.DataAposta });
            command.Parameters.Add(new SQLiteParameter("@DataResultado", DbType.DateTime) { Value = aposta.DataResultado.HasValue ? (object)aposta.DataResultado.Value : DBNull.Value });

            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            aposta.Id = id;

            return aposta;
        }

        public async Task<Aposta?> GetByIdAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Apostas WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapAposta(reader);
            }

            return null;
        }

        public async Task<List<Aposta>> GetByUsuarioIdAsync(int usuarioId)
        {
            var apostas = new List<Aposta>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Apostas WHERE UsuarioId = @UsuarioId ORDER BY DataAposta DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@UsuarioId", DbType.Int32) { Value = usuarioId });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                apostas.Add(MapAposta(reader));
            }

            return apostas;
        }

        public async Task<List<Aposta>> GetAllAsync()
        {
            var apostas = new List<Aposta>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Apostas ORDER BY DataAposta DESC";
            using var command = new SQLiteCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                apostas.Add(MapAposta(reader));
            }

            return apostas;
        }

        public async Task<Aposta> UpdateAsync(Aposta aposta)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Apostas 
                SET UsuarioId = @UsuarioId, TipoAposta = @TipoAposta, Valor = @Valor, 
                    Multiplicador = @Multiplicador, Status = @Status, ValorGanho = @ValorGanho, 
                    DataAposta = @DataAposta, DataResultado = @DataResultado
                WHERE Id = @Id";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = aposta.Id });
            command.Parameters.Add(new SQLiteParameter("@UsuarioId", DbType.Int32) { Value = aposta.UsuarioId });
            command.Parameters.Add(new SQLiteParameter("@TipoAposta", DbType.String) { Value = aposta.TipoAposta });
            command.Parameters.Add(new SQLiteParameter("@Valor", DbType.Decimal) { Value = aposta.Valor });
            command.Parameters.Add(new SQLiteParameter("@Multiplicador", DbType.Decimal) { Value = aposta.Multiplicador });
            command.Parameters.Add(new SQLiteParameter("@Status", DbType.String) { Value = aposta.Status });
            command.Parameters.Add(new SQLiteParameter("@ValorGanho", DbType.Decimal) { Value = aposta.ValorGanho.HasValue ? (object)aposta.ValorGanho.Value : DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@DataAposta", DbType.DateTime) { Value = aposta.DataAposta });
            command.Parameters.Add(new SQLiteParameter("@DataResultado", DbType.DateTime) { Value = aposta.DataResultado.HasValue ? (object)aposta.DataResultado.Value : DBNull.Value });

            await command.ExecuteNonQueryAsync();
            return aposta;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "DELETE FROM Apostas WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<List<Aposta>> GetByStatusAsync(string status)
        {
            var apostas = new List<Aposta>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Apostas WHERE Status = @Status ORDER BY DataAposta DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Status", DbType.String) { Value = status });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                apostas.Add(MapAposta(reader));
            }

            return apostas;
        }

        public async Task<List<Aposta>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim)
        {
            var apostas = new List<Aposta>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Apostas WHERE DataAposta BETWEEN @DataInicio AND @DataFim ORDER BY DataAposta DESC";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@DataInicio", DbType.DateTime) { Value = dataInicio });
            command.Parameters.Add(new SQLiteParameter("@DataFim", DbType.DateTime) { Value = dataFim });

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                apostas.Add(MapAposta(reader));
            }

            return apostas;
        }

        private Aposta MapAposta(System.Data.Common.DbDataReader reader)
        {
            return new Aposta
            {
                Id = Convert.ToInt32(reader["Id"]),
                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                TipoAposta = Convert.ToString(reader["TipoAposta"]) ?? string.Empty,
                Valor = Convert.ToDecimal(reader["Valor"]),
                Multiplicador = Convert.ToDecimal(reader["Multiplicador"]),
                Status = Convert.ToString(reader["Status"]) ?? string.Empty,
                ValorGanho = reader.IsDBNull(6) ? null : Convert.ToDecimal(reader["ValorGanho"]),
                DataAposta = Convert.ToDateTime(reader["DataAposta"]),
                DataResultado = reader.IsDBNull(8) ? null : Convert.ToDateTime(reader["DataResultado"])
            };
        }
    }
}
