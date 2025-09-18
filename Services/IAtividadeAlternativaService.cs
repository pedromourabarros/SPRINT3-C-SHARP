using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para serviço de atividades alternativas
    /// </summary>
    public interface IAtividadeAlternativaService
    {
        Task<List<AtividadeAlternativa>> ListarAtividadesAsync();
        Task<List<AtividadeAlternativa>> ListarAtividadesPorCategoriaAsync(CategoriaAtividade categoria);
        Task<List<AtividadeAlternativa>> ListarAtividadesPorCustoAsync(decimal custoMaximo);
        Task<List<AtividadeAlternativa>> ListarAtividadesPorDuracaoAsync(int duracaoMaxima);
        Task<List<AtividadeAlternativa>> ListarAtividadesPorDificuldadeAsync(int nivelMaximo);
        Task<AtividadeAlternativa> SugerirAtividadePersonalizadaAsync(int usuarioId);
        Task<List<AtividadeAlternativa>> BuscarAtividadesAsync(string termo);
        Task<AtividadeAlternativa> ObterAtividadePorIdAsync(int id);
        Task<bool> CriarAtividadeAsync(AtividadeAlternativa atividade);
        Task<bool> AtualizarAtividadeAsync(AtividadeAlternativa atividade);
        Task<bool> DesativarAtividadeAsync(int id);
        Task<List<AtividadeAlternativa>> ObterAtividadesRecomendadasAsync(int usuarioId);
    }
}
