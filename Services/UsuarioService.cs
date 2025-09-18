using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pelas regras de negócio de usuários
    /// </summary>
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHistoricoRepository _historicoRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository, IHistoricoRepository historicoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task<Usuario> CriarUsuarioAsync(string nome, string email, decimal saldoInicial = 0, string telefone = "")
        {
            // Validações centralizadas
            var validacaoNome = ValidationService.ValidarNome(nome);
            if (!validacaoNome.IsValid)
                throw new ArgumentException(validacaoNome.ErrorMessage, nameof(nome));

            var validacaoEmail = ValidationService.ValidarEmail(email);
            if (!validacaoEmail.IsValid)
                throw new ArgumentException(validacaoEmail.ErrorMessage, nameof(email));

            var validacaoTelefone = ValidationService.ValidarTelefone(telefone);
            if (!validacaoTelefone.IsValid)
                throw new ArgumentException(validacaoTelefone.ErrorMessage, nameof(telefone));

            var validacaoSaldo = ValidationService.ValidarValorMonetario(saldoInicial, "Saldo inicial");
            if (!validacaoSaldo.IsValid)
                throw new ArgumentException(validacaoSaldo.ErrorMessage, nameof(saldoInicial));

            // Verificar se email já existe
            if (await _usuarioRepository.EmailExistsAsync(email))
                throw new InvalidOperationException("Email já cadastrado no sistema");

            var usuario = new Usuario(nome, email, saldoInicial);
            usuario.Telefone = telefone;
            usuario.CalcularPontuacaoRisco(); // Garantir que a pontuação seja calculada
            var usuarioCriado = await _usuarioRepository.CreateAsync(usuario);

            // Registrar no histórico se houver saldo inicial
            if (saldoInicial > 0)
            {
                var historico = new Historico(
                    usuarioCriado.Id,
                    "Deposito",
                    saldoInicial,
                    "Saldo inicial do cadastro",
                    0,
                    saldoInicial
                );
                await _historicoRepository.CreateAsync(historico);
            }

            return usuarioCriado;
        }

        public async Task<Usuario?> ObterUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario?> ObterUsuarioPorEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<List<Usuario>> ListarUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task<Usuario> AtualizarUsuarioAsync(Usuario usuario)
        {
            if (!await _usuarioRepository.ExistsAsync(usuario.Id))
                throw new InvalidOperationException("Usuário não encontrado");

            // Verificar se email já existe em outro usuário
            var usuarioExistente = await _usuarioRepository.GetByEmailAsync(usuario.Email);
            if (usuarioExistente != null && usuarioExistente.Id != usuario.Id)
                throw new InvalidOperationException("Email já cadastrado para outro usuário");

            return await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task<bool> DesativarUsuarioAsync(int id)
        {
            if (!await _usuarioRepository.ExistsAsync(id))
                return false;

            return await _usuarioRepository.DeleteAsync(id);
        }

        public async Task<bool> DepositarAsync(int usuarioId, decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor do depósito deve ser positivo", nameof(valor));

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário inativo não pode receber depósitos");

            var saldoAnterior = usuario.Saldo;
            usuario.Saldo += valor;
            await _usuarioRepository.UpdateAsync(usuario);

            // Registrar no histórico
            var historico = new Historico(
                usuarioId,
                "Deposito",
                valor,
                $"Depósito realizado - R$ {valor:F2}",
                saldoAnterior,
                usuario.Saldo
            );
            await _historicoRepository.CreateAsync(historico);

            return true;
        }

        public async Task<bool> SacarAsync(int usuarioId, decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException("Valor do saque deve ser positivo", nameof(valor));

            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário inativo não pode realizar saques");

            if (usuario.Saldo < valor)
                throw new InvalidOperationException("Saldo insuficiente para o saque");

            var saldoAnterior = usuario.Saldo;
            usuario.Saldo -= valor;
            await _usuarioRepository.UpdateAsync(usuario);

            // Registrar no histórico
            var historico = new Historico(
                usuarioId,
                "Saque",
                valor,
                $"Saque realizado - R$ {valor:F2}",
                saldoAnterior,
                usuario.Saldo
            );
            await _historicoRepository.CreateAsync(historico);

            return true;
        }

        public async Task<bool> UsuarioExisteAsync(int id)
        {
            return await _usuarioRepository.ExistsAsync(id);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _usuarioRepository.EmailExistsAsync(email);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExcluirUsuarioDefinitivamenteAsync(int usuarioId)
        {
            try
            {
                // Verificar se usuário existe
                var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
                if (usuario == null)
                    return false;

                // Excluir usuário definitivamente (LGPD - direito ao esquecimento)
                await _usuarioRepository.DeleteAsync(usuarioId);
                
                // Registrar exclusão no histórico (se necessário)
                var historico = new Historico(
                    usuarioId,
                    "ExclusaoDefinitiva",
                    0,
                    "Usuário excluído definitivamente conforme LGPD",
                    0,
                    0
                );
                await _historicoRepository.CreateAsync(historico);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
