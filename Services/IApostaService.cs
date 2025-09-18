using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para operações de serviço de apostas
    /// </summary>
    public interface IApostaService
    {
        Task<Aposta> RealizarApostaAsync(int usuarioId, string tipoAposta, decimal valor, decimal multiplicador);
        Task<Aposta?> ObterApostaPorIdAsync(int id);
        Task<List<Aposta>> ObterApostasPorUsuarioAsync(int usuarioId);
        Task<List<Aposta>> ListarApostasAsync();
        Task<Aposta> FinalizarApostaAsync(int apostaId, bool ganhou);
        Task<List<Aposta>> ObterApostasPorStatusAsync(string status);
        Task<List<Aposta>> ObterApostasPorPeriodoAsync(System.DateTime dataInicio, System.DateTime dataFim);
        Task<decimal> CalcularTotalApostadoAsync(int usuarioId);
        Task<decimal> CalcularTotalGanhoAsync(int usuarioId);
    }
}
