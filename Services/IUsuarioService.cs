using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para operações de serviço de usuários
    /// </summary>
    public interface IUsuarioService
    {
        Task<Usuario> CriarUsuarioAsync(string nome, string email, decimal saldoInicial = 0, string telefone = "");
        Task<Usuario?> ObterUsuarioPorIdAsync(int id);
        Task<Usuario?> ObterUsuarioPorEmailAsync(string email);
        Task<List<Usuario>> ListarUsuariosAsync();
        Task<Usuario> AtualizarUsuarioAsync(Usuario usuario);
        Task<bool> DesativarUsuarioAsync(int id);
        Task<bool> DepositarAsync(int usuarioId, decimal valor);
        Task<bool> SacarAsync(int usuarioId, decimal valor);
        Task<bool> UsuarioExisteAsync(int id);
        Task<bool> EmailExisteAsync(string email);
        Task<bool> ExcluirUsuarioDefinitivamenteAsync(int usuarioId);
    }
}
