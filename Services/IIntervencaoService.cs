using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para serviço de intervenções
    /// </summary>
    public interface IIntervencaoService
    {
        Task<Intervencao> CriarIntervencaoAsync(int usuarioId, TipoIntervencao tipo, string titulo, string mensagem, int prioridade = 1);
        Task<List<Intervencao>> ObterIntervencoesPendentesAsync(int usuarioId);
        Task<List<Intervencao>> ObterIntervencoesPorTipoAsync(TipoIntervencao tipo);
        Task<bool> MarcarComoVisualizadaAsync(int intervencaoId);
        Task<bool> MarcarComoAceitaAsync(int intervencaoId);
        Task<List<Intervencao>> ObterIntervencoesUrgentesAsync();
        Task<Intervencao> CriarAlertaComportamentoAsync(int usuarioId, string descricao);
        Task<Intervencao> CriarSugestaoAtividadeAsync(int usuarioId, string atividade);
        Task<Intervencao> CriarLimiteApostasAsync(int usuarioId, decimal limite);
        Task<Intervencao> CriarPausaObrigatoriaAsync(int usuarioId, int horas);
        Task<Intervencao> CriarContatoApoioAsync(int usuarioId);
        Task<Intervencao> CriarEducacaoRiscosAsync(int usuarioId);
        Task<Intervencao> CriarSimulacaoInvestimentoAsync(int usuarioId, decimal valor);
        Task<Intervencao> CriarEstatisticasConscientizacaoAsync(int usuarioId);
        Task<Intervencao> CriarLembreteResponsabilidadeAsync(int usuarioId);
        Task<Intervencao> CriarConviteTerapiaAsync(int usuarioId);
    }
}
