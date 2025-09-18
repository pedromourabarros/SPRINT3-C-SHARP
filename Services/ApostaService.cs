using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pelas regras de negócio de apostas
    /// </summary>
    public class ApostaService : IApostaService
    {
        private readonly IApostaRepository _apostaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IHistoricoRepository _historicoRepository;

        public ApostaService(IApostaRepository apostaRepository, IUsuarioRepository usuarioRepository, IHistoricoRepository historicoRepository)
        {
            _apostaRepository = apostaRepository;
            _usuarioRepository = usuarioRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task<Aposta> RealizarApostaAsync(int usuarioId, string tipoAposta, decimal valor, decimal multiplicador)
        {
            // Validações de negócio
            if (valor <= 0)
                throw new ArgumentException("Valor da aposta deve ser positivo", nameof(valor));

            if (multiplicador <= 0)
                throw new ArgumentException("Multiplicador deve ser positivo", nameof(multiplicador));

            if (string.IsNullOrWhiteSpace(tipoAposta))
                throw new ArgumentException("Tipo de aposta não pode ser vazio", nameof(tipoAposta));

            // Verificar se usuário existe e está ativo
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
                throw new InvalidOperationException("Usuário não encontrado");

            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário inativo não pode realizar apostas");

            // Verificar saldo suficiente
            if (usuario.Saldo < valor)
                throw new InvalidOperationException("Saldo insuficiente para realizar a aposta");

            // Criar aposta
            var aposta = new Aposta(usuarioId, tipoAposta, valor, multiplicador);
            var apostaCriada = await _apostaRepository.CreateAsync(aposta);

            // Debitar valor do saldo do usuário
            var saldoAnterior = usuario.Saldo;
            usuario.Saldo -= valor;
            await _usuarioRepository.UpdateAsync(usuario);

            // Registrar no histórico
            var historico = new Historico(
                usuarioId,
                "Aposta",
                valor,
                $"Aposta realizada - {tipoAposta} - R$ {valor:F2} (Multiplicador: {multiplicador}x)",
                saldoAnterior,
                usuario.Saldo
            );
            await _historicoRepository.CreateAsync(historico);

            return apostaCriada;
        }

        public async Task<Aposta?> ObterApostaPorIdAsync(int id)
        {
            return await _apostaRepository.GetByIdAsync(id);
        }

        public async Task<List<Aposta>> ObterApostasPorUsuarioAsync(int usuarioId)
        {
            return await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<List<Aposta>> ListarApostasAsync()
        {
            return await _apostaRepository.GetAllAsync();
        }

        public async Task<Aposta> FinalizarApostaAsync(int apostaId, bool ganhou)
        {
            var aposta = await _apostaRepository.GetByIdAsync(apostaId);
            if (aposta == null)
                throw new InvalidOperationException("Aposta não encontrada");

            if (aposta.Status != "Pendente")
                throw new InvalidOperationException("Aposta já foi finalizada");

            // Finalizar aposta
            aposta.FinalizarAposta(ganhou);
            await _apostaRepository.UpdateAsync(aposta);

            // Se ganhou, creditar valor no saldo do usuário
            if (ganhou)
            {
                var usuario = await _usuarioRepository.GetByIdAsync(aposta.UsuarioId);
                if (usuario != null)
                {
                    var saldoAnterior = usuario.Saldo;
                    usuario.Saldo += aposta.ValorGanho!.Value;
                    await _usuarioRepository.UpdateAsync(usuario);

                    // Registrar no histórico
                    var historico = new Historico(
                        aposta.UsuarioId,
                        "Ganho",
                        aposta.ValorGanho.Value,
                        $"Ganho de aposta - {aposta.TipoAposta} - R$ {aposta.ValorGanho.Value:F2}",
                        saldoAnterior,
                        usuario.Saldo
                    );
                    await _historicoRepository.CreateAsync(historico);
                }
            }
            else
            {
                // Registrar perda no histórico
                var usuario = await _usuarioRepository.GetByIdAsync(aposta.UsuarioId);
                if (usuario != null)
                {
                    var historico = new Historico(
                        aposta.UsuarioId,
                        "Perda",
                        aposta.Valor,
                        $"Perda de aposta - {aposta.TipoAposta} - R$ {aposta.Valor:F2}",
                        usuario.Saldo,
                        usuario.Saldo
                    );
                    await _historicoRepository.CreateAsync(historico);
                }
            }

            return aposta;
        }

        public async Task<List<Aposta>> ObterApostasPorStatusAsync(string status)
        {
            return await _apostaRepository.GetByStatusAsync(status);
        }

        public async Task<List<Aposta>> ObterApostasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _apostaRepository.GetByDataRangeAsync(dataInicio, dataFim);
        }

        public async Task<decimal> CalcularTotalApostadoAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            return apostas.Sum(a => a.Valor);
        }

        public async Task<decimal> CalcularTotalGanhoAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            return apostas.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue)
                         .Sum(a => a.ValorGanho!.Value);
        }
    }
}
