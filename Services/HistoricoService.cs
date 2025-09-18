using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pelas regras de negócio de histórico
    /// </summary>
    public class HistoricoService : IHistoricoService
    {
        private readonly IHistoricoRepository _historicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public HistoricoService(IHistoricoRepository historicoRepository, IUsuarioRepository usuarioRepository)
        {
            _historicoRepository = historicoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Historico>> ObterHistoricoPorUsuarioAsync(int usuarioId)
        {
            return await _historicoRepository.GetByUsuarioIdAsync(usuarioId);
        }

        public async Task<List<Historico>> ObterHistoricoCompletoAsync()
        {
            return await _historicoRepository.GetAllAsync();
        }

        public async Task<List<Historico>> ObterHistoricoPorTipoAsync(string tipoOperacao)
        {
            return await _historicoRepository.GetByTipoOperacaoAsync(tipoOperacao);
        }

        public async Task<List<Historico>> ObterHistoricoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _historicoRepository.GetByDataRangeAsync(dataInicio, dataFim);
        }

        public async Task<decimal> CalcularSaldoAtualAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            return usuario?.Saldo ?? 0;
        }

        public async Task<List<Historico>> ObterUltimasOperacoesAsync(int usuarioId, int quantidade)
        {
            var historico = await _historicoRepository.GetByUsuarioIdAsync(usuarioId);
            return historico.Take(quantidade).ToList();
        }
    }
}
