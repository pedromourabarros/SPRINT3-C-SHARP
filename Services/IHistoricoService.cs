using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para operações de serviço de histórico
    /// </summary>
    public interface IHistoricoService
    {
        Task<List<Historico>> ObterHistoricoPorUsuarioAsync(int usuarioId);
        Task<List<Historico>> ObterHistoricoCompletoAsync();
        Task<List<Historico>> ObterHistoricoPorTipoAsync(string tipoOperacao);
        Task<List<Historico>> ObterHistoricoPorPeriodoAsync(System.DateTime dataInicio, System.DateTime dataFim);
        Task<decimal> CalcularSaldoAtualAsync(int usuarioId);
        Task<List<Historico>> ObterUltimasOperacoesAsync(int usuarioId, int quantidade);
    }
}
