using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Interface para operações de repositório de histórico
    /// </summary>
    public interface IHistoricoRepository
    {
        Task<Historico> CreateAsync(Historico historico);
        Task<List<Historico>> GetByUsuarioIdAsync(int usuarioId);
        Task<List<Historico>> GetAllAsync();
        Task<List<Historico>> GetByTipoOperacaoAsync(string tipoOperacao);
        Task<List<Historico>> GetByDataRangeAsync(System.DateTime dataInicio, System.DateTime dataFim);
    }
}
