using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço que implementa algoritmos de análise comportamental para identificar padrões de risco
    /// </summary>
    public class DetecaoComportamentoService : IDetecaoComportamentoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IApostaRepository _apostaRepository;
        private readonly IHistoricoRepository _historicoRepository;

        public DetecaoComportamentoService(IUsuarioRepository usuarioRepository, IApostaRepository apostaRepository, IHistoricoRepository historicoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _apostaRepository = apostaRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task<List<ComportamentoRisco>> DetectarComportamentosRiscoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return new List<ComportamentoRisco>();

            return await DetectarComportamentosRiscoAsync(usuario);
        }

        public async Task<List<ComportamentoRisco>> DetectarComportamentosRiscoAsync(Usuario usuario)
        {
            var comportamentos = new List<ComportamentoRisco>();

            // Verificar apostas frequentes
            if (await VerificarApostasFrequentesAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.ApostasFrequentes,
                    "Usuário realizou mais de 5 apostas em um dia",
                    7
                ));
            }

            // Verificar valores altos
            if (await VerificarValoresAltosAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.ValoresAltos,
                    "Usuário apostou mais de 10% do saldo em um dia",
                    8
                ));
            }

            // Verificar apostas consecutivas
            if (await VerificarApostasConsecutivasAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.ApostasConsecutivas,
                    "Usuário apostou por mais de 3 dias consecutivos",
                    6
                ));
            }

            // Verificar apostas noturnas
            if (await VerificarApostasNoturnasAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.ApostasNoturnas,
                    "Usuário realizou apostas entre 22h e 6h",
                    5
                ));
            }

            // Verificar perdas recorrentes
            if (await VerificarPerdasRecorrentesAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.PerdasRecorrentes,
                    "Usuário perdeu em mais de 70% das apostas recentes",
                    9
                ));
            }

            // Verificar tentativa de recuperação
            if (await VerificarTentativaRecuperacaoAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.TentativaRecuperacao,
                    "Usuário aumentou valores após perdas",
                    8
                ));
            }

            // Verificar apostas emocionais
            if (await VerificarApostasEmocionaisAsync(usuario.Id))
            {
                comportamentos.Add(new ComportamentoRisco(
                    usuario.Id,
                    TipoComportamento.ApostasEmocionais,
                    "Usuário apostou imediatamente após uma perda",
                    7
                ));
            }

            return comportamentos;
        }

        public async Task<bool> VerificarApostasFrequentesAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var hoje = DateTime.Today;
            
            var apostasHoje = apostas.Count(a => a.DataAposta.Date == hoje);
            return apostasHoje > 5;
        }

        public async Task<bool> VerificarValoresAltosAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return false;

            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var hoje = DateTime.Today;
            
            var valorApostadoHoje = apostas
                .Where(a => a.DataAposta.Date == hoje)
                .Sum(a => a.Valor);

            return valorApostadoHoje > usuario.Saldo * 0.1m;
        }

        public async Task<bool> VerificarApostasConsecutivasAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var datasApostas = apostas.Select(a => a.DataAposta.Date).Distinct().OrderBy(d => d).ToList();
            
            if (datasApostas.Count < 3) return false;

            int diasConsecutivos = 1;
            for (int i = 1; i < datasApostas.Count; i++)
            {
                if ((datasApostas[i] - datasApostas[i - 1]).Days == 1)
                {
                    diasConsecutivos++;
                    if (diasConsecutivos >= 3) return true;
                }
                else
                {
                    diasConsecutivos = 1;
                }
            }

            return false;
        }

        public async Task<bool> VerificarApostasNoturnasAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var hoje = DateTime.Today;
            
            var apostasNoturnas = apostas.Count(a => 
                a.DataAposta.Date == hoje && 
                (a.DataAposta.Hour >= 22 || a.DataAposta.Hour < 6));

            return apostasNoturnas > 0;
        }

        public async Task<bool> VerificarPerdasRecorrentesAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var apostasRecentes = apostas
                .Where(a => a.DataAposta >= DateTime.Now.AddDays(-7))
                .ToList();

            if (apostasRecentes.Count < 5) return false;

            var apostasPerdidas = apostasRecentes.Count(a => a.Status == "Perdeu");
            var taxaPerda = (decimal)apostasPerdidas / apostasRecentes.Count;

            return taxaPerda > 0.7m;
        }

        public async Task<bool> VerificarTentativaRecuperacaoAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var apostasRecentes = apostas
                .Where(a => a.DataAposta >= DateTime.Now.AddDays(-3))
                .OrderBy(a => a.DataAposta)
                .ToList();

            if (apostasRecentes.Count < 3) return false;

            // Verificar se houve aumento de valores após perdas
            for (int i = 1; i < apostasRecentes.Count; i++)
            {
                var apostaAnterior = apostasRecentes[i - 1];
                var apostaAtual = apostasRecentes[i];

                if (apostaAnterior.Status == "Perdeu" && 
                    apostaAtual.Valor > apostaAnterior.Valor * 1.5m)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> VerificarApostasEmocionaisAsync(int usuarioId)
        {
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var apostasRecentes = apostas
                .Where(a => a.DataAposta >= DateTime.Now.AddDays(-1))
                .OrderBy(a => a.DataAposta)
                .ToList();

            if (apostasRecentes.Count < 2) return false;

            // Verificar se houve aposta imediatamente após perda
            for (int i = 1; i < apostasRecentes.Count; i++)
            {
                var apostaAnterior = apostasRecentes[i - 1];
                var apostaAtual = apostasRecentes[i];

                if (apostaAnterior.Status == "Perdeu" && 
                    (apostaAtual.DataAposta - apostaAnterior.DataAposta).TotalMinutes < 30)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<int> CalcularPontuacaoRiscoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return 0;

            usuario.CalcularPontuacaoRisco();
            await _usuarioRepository.UpdateAsync(usuario);

            return usuario.PontuacaoRisco;
        }

        public async Task<NivelRisco> ClassificarNivelRiscoAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return NivelRisco.Baixo;

            await CalcularPontuacaoRiscoAsync(usuarioId);
            return usuario.NivelRisco;
        }

        public async Task<List<ComportamentoRisco>> ObterComportamentosRiscoAsync(int usuarioId)
        {
            // Implementar busca no banco de dados
            return new List<ComportamentoRisco>();
        }

        public async Task<List<ComportamentoRisco>> ObterComportamentosRiscoPorTipoAsync(TipoComportamento tipo)
        {
            // Implementar busca no banco de dados
            return new List<ComportamentoRisco>();
        }

        public async Task<bool> MarcarComportamentoNotificadoAsync(int comportamentoId)
        {
            // Implementar atualização no banco de dados
            return true;
        }

        public async Task<bool> MarcarAcaoExecutadaAsync(int comportamentoId)
        {
            // Implementar atualização no banco de dados
            return true;
        }
    }
}
