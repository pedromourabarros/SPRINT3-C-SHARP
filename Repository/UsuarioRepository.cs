using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Implementação do repositório de usuários com SQLite
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Usuario> CreateAsync(Usuario usuario)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Usuarios (Nome, Email, Saldo, DataCadastro, Ativo, NivelRisco, PontuacaoRisco, UltimaAvaliacao, RecebeAlertas, AceitaApoio, Telefone, DataUltimaAposta, TotalApostasHoje, ValorApostadoHoje, DiasConsecutivosApostando, ConsentimentoAceito, DataConsentimento)
                VALUES (@Nome, @Email, @Saldo, @DataCadastro, @Ativo, @NivelRisco, @PontuacaoRisco, @UltimaAvaliacao, @RecebeAlertas, @AceitaApoio, @Telefone, @DataUltimaAposta, @TotalApostasHoje, @ValorApostadoHoje, @DiasConsecutivosApostando, @ConsentimentoAceito, @DataConsentimento);
                SELECT last_insert_rowid();";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Nome", DbType.String) { Value = usuario.Nome });
            command.Parameters.Add(new SQLiteParameter("@Email", DbType.String) { Value = usuario.Email });
            command.Parameters.Add(new SQLiteParameter("@Saldo", DbType.Decimal) { Value = usuario.Saldo });
            command.Parameters.Add(new SQLiteParameter("@DataCadastro", DbType.DateTime) { Value = usuario.DataCadastro });
            command.Parameters.Add(new SQLiteParameter("@Ativo", DbType.Boolean) { Value = usuario.Ativo });
            command.Parameters.Add(new SQLiteParameter("@NivelRisco", DbType.Int32) { Value = (int)usuario.NivelRisco });
            command.Parameters.Add(new SQLiteParameter("@PontuacaoRisco", DbType.Int32) { Value = usuario.PontuacaoRisco });
            command.Parameters.Add(new SQLiteParameter("@UltimaAvaliacao", DbType.DateTime) { Value = usuario.UltimaAvaliacao });
            command.Parameters.Add(new SQLiteParameter("@RecebeAlertas", DbType.Boolean) { Value = usuario.RecebeAlertas });
            command.Parameters.Add(new SQLiteParameter("@AceitaApoio", DbType.Boolean) { Value = usuario.AceitaApoio });
            command.Parameters.Add(new SQLiteParameter("@Telefone", DbType.String) { Value = usuario.Telefone ?? (object)DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@DataUltimaAposta", DbType.DateTime) { Value = usuario.DataUltimaAposta ?? (object)DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@TotalApostasHoje", DbType.Int32) { Value = usuario.TotalApostasHoje });
            command.Parameters.Add(new SQLiteParameter("@ValorApostadoHoje", DbType.Decimal) { Value = usuario.ValorApostadoHoje });
            command.Parameters.Add(new SQLiteParameter("@DiasConsecutivosApostando", DbType.Int32) { Value = usuario.DiasConsecutivosApostando });
            command.Parameters.Add(new SQLiteParameter("@ConsentimentoAceito", DbType.Boolean) { Value = usuario.ConsentimentoAceito });
            command.Parameters.Add(new SQLiteParameter("@DataConsentimento", DbType.DateTime) { Value = usuario.DataConsentimento ?? (object)DBNull.Value });

            var id = Convert.ToInt32(await command.ExecuteScalarAsync());
            usuario.Id = id;

            return usuario;
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Usuarios WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUsuario(reader);
            }

            return null;
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Usuarios WHERE Email = @Email";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Email", DbType.String) { Value = email });

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapUsuario(reader);
            }

            return null;
        }

        public async Task<List<Usuario>> GetAllAsync()
        {
            var usuarios = new List<Usuario>();

            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT * FROM Usuarios ORDER BY Nome";
            using var command = new SQLiteCommand(query, connection);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                usuarios.Add(MapUsuario(reader));
            }

            return usuarios;
        }

        public async Task<Usuario> UpdateAsync(Usuario usuario)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = @"
                UPDATE Usuarios 
                SET Nome = @Nome, Email = @Email, Saldo = @Saldo, Ativo = @Ativo, 
                    NivelRisco = @NivelRisco, PontuacaoRisco = @PontuacaoRisco, 
                    UltimaAvaliacao = @UltimaAvaliacao, RecebeAlertas = @RecebeAlertas, 
                    AceitaApoio = @AceitaApoio, Telefone = @Telefone, 
                    DataUltimaAposta = @DataUltimaAposta, TotalApostasHoje = @TotalApostasHoje, 
                    ValorApostadoHoje = @ValorApostadoHoje, DiasConsecutivosApostando = @DiasConsecutivosApostando,
                    ConsentimentoAceito = @ConsentimentoAceito, DataConsentimento = @DataConsentimento
                WHERE Id = @Id";

            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = usuario.Id });
            command.Parameters.Add(new SQLiteParameter("@Nome", DbType.String) { Value = usuario.Nome });
            command.Parameters.Add(new SQLiteParameter("@Email", DbType.String) { Value = usuario.Email });
            command.Parameters.Add(new SQLiteParameter("@Saldo", DbType.Decimal) { Value = usuario.Saldo });
            command.Parameters.Add(new SQLiteParameter("@Ativo", DbType.Boolean) { Value = usuario.Ativo });
            command.Parameters.Add(new SQLiteParameter("@NivelRisco", DbType.Int32) { Value = (int)usuario.NivelRisco });
            command.Parameters.Add(new SQLiteParameter("@PontuacaoRisco", DbType.Int32) { Value = usuario.PontuacaoRisco });
            command.Parameters.Add(new SQLiteParameter("@UltimaAvaliacao", DbType.DateTime) { Value = usuario.UltimaAvaliacao });
            command.Parameters.Add(new SQLiteParameter("@RecebeAlertas", DbType.Boolean) { Value = usuario.RecebeAlertas });
            command.Parameters.Add(new SQLiteParameter("@AceitaApoio", DbType.Boolean) { Value = usuario.AceitaApoio });
            command.Parameters.Add(new SQLiteParameter("@Telefone", DbType.String) { Value = usuario.Telefone ?? (object)DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@DataUltimaAposta", DbType.DateTime) { Value = usuario.DataUltimaAposta ?? (object)DBNull.Value });
            command.Parameters.Add(new SQLiteParameter("@TotalApostasHoje", DbType.Int32) { Value = usuario.TotalApostasHoje });
            command.Parameters.Add(new SQLiteParameter("@ValorApostadoHoje", DbType.Decimal) { Value = usuario.ValorApostadoHoje });
            command.Parameters.Add(new SQLiteParameter("@DiasConsecutivosApostando", DbType.Int32) { Value = usuario.DiasConsecutivosApostando });
            command.Parameters.Add(new SQLiteParameter("@ConsentimentoAceito", DbType.Boolean) { Value = usuario.ConsentimentoAceito });
            command.Parameters.Add(new SQLiteParameter("@DataConsentimento", DbType.DateTime) { Value = usuario.DataConsentimento ?? (object)DBNull.Value });

            await command.ExecuteNonQueryAsync();
            return usuario;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "UPDATE Usuarios SET Ativo = 0 WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM Usuarios WHERE Id = @Id";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Id", DbType.Int32) { Value = id });

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = new SQLiteConnection(_connectionString);
            await connection.OpenAsync();

            var query = "SELECT COUNT(1) FROM Usuarios WHERE Email = @Email";
            using var command = new SQLiteCommand(query, connection);
            command.Parameters.Add(new SQLiteParameter("@Email", DbType.String) { Value = email });

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        private Usuario MapUsuario(System.Data.Common.DbDataReader reader)
        {
            return new Usuario
            {
                Id = Convert.ToInt32(reader["Id"]),
                Nome = Convert.ToString(reader["Nome"]) ?? string.Empty,
                Email = Convert.ToString(reader["Email"]) ?? string.Empty,
                Saldo = Convert.ToDecimal(reader["Saldo"]),
                DataCadastro = Convert.ToDateTime(reader["DataCadastro"]),
                Ativo = Convert.ToBoolean(reader["Ativo"]),
                NivelRisco = (NivelRisco)Convert.ToInt32(reader["NivelRisco"]),
                PontuacaoRisco = Convert.ToInt32(reader["PontuacaoRisco"]),
                UltimaAvaliacao = Convert.ToDateTime(reader["UltimaAvaliacao"]),
                RecebeAlertas = Convert.ToBoolean(reader["RecebeAlertas"]),
                AceitaApoio = Convert.ToBoolean(reader["AceitaApoio"]),
                Telefone = reader.IsDBNull(11) ? string.Empty : Convert.ToString(reader["Telefone"]) ?? string.Empty,
                DataUltimaAposta = reader.IsDBNull(12) ? null : Convert.ToDateTime(reader["DataUltimaAposta"]),
                TotalApostasHoje = Convert.ToInt32(reader["TotalApostasHoje"]),
                ValorApostadoHoje = Convert.ToDecimal(reader["ValorApostadoHoje"]),
                DiasConsecutivosApostando = Convert.ToInt32(reader["DiasConsecutivosApostando"]),
                ConsentimentoAceito = Convert.ToBoolean(reader["ConsentimentoAceito"]),
                DataConsentimento = reader.IsDBNull(17) ? null : Convert.ToDateTime(reader["DataConsentimento"])
            };
        }
    }
}
