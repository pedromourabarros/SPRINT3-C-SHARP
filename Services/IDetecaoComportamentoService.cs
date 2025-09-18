using System.Collections.Generic;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Interface para serviço de detecção de comportamento compulsivo
    /// </summary>
    public interface IDetecaoComportamentoService
    {
        Task<List<ComportamentoRisco>> DetectarComportamentosRiscoAsync(int usuarioId);
        Task<List<ComportamentoRisco>> DetectarComportamentosRiscoAsync(Usuario usuario);
        Task<bool> VerificarApostasFrequentesAsync(int usuarioId);
        Task<bool> VerificarValoresAltosAsync(int usuarioId);
        Task<bool> VerificarApostasConsecutivasAsync(int usuarioId);
        Task<bool> VerificarApostasNoturnasAsync(int usuarioId);
        Task<bool> VerificarPerdasRecorrentesAsync(int usuarioId);
        Task<bool> VerificarTentativaRecuperacaoAsync(int usuarioId);
        Task<bool> VerificarApostasEmocionaisAsync(int usuarioId);
        Task<int> CalcularPontuacaoRiscoAsync(int usuarioId);
        Task<NivelRisco> ClassificarNivelRiscoAsync(int usuarioId);
        Task<List<ComportamentoRisco>> ObterComportamentosRiscoAsync(int usuarioId);
        Task<List<ComportamentoRisco>> ObterComportamentosRiscoPorTipoAsync(TipoComportamento tipo);
        Task<bool> MarcarComportamentoNotificadoAsync(int comportamentoId);
        Task<bool> MarcarAcaoExecutadaAsync(int comportamentoId);
    }
}
