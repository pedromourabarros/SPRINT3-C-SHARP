using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Repository
{
    /// <summary>
    /// Interface para operações de repositório de apostas
    /// </summary>
    public interface IApostaRepository
    {
        Task<Aposta> CreateAsync(Aposta aposta);
        Task<Aposta?> GetByIdAsync(int id);
        Task<List<Aposta>> GetByUsuarioIdAsync(int usuarioId);
        Task<List<Aposta>> GetAllAsync();
        Task<Aposta> UpdateAsync(Aposta aposta);
        Task<bool> DeleteAsync(int id);
        Task<List<Aposta>> GetByStatusAsync(string status);
        Task<List<Aposta>> GetByDataRangeAsync(DateTime dataInicio, DateTime dataFim);
    }
}
