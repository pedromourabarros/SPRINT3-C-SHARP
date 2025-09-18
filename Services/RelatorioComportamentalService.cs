using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApostasCompulsivas.Models;
using ApostasCompulsivas.Repository;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço responsável pelos relatórios comportamentais
    /// </summary>
    public class RelatorioComportamentalService : IRelatorioComportamentalService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IApostaRepository _apostaRepository;
        private readonly IHistoricoRepository _historicoRepository;

        public RelatorioComportamentalService(IUsuarioRepository usuarioRepository, IApostaRepository apostaRepository, IHistoricoRepository historicoRepository)
        {
            _usuarioRepository = usuarioRepository;
            _apostaRepository = apostaRepository;
            _historicoRepository = historicoRepository;
        }

        public async Task<RelatorioComportamental> GerarRelatorioAsync(int usuarioId, DateTime dataInicio, DateTime dataFim)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return null;

            var apostas = await _apostaRepository.GetByDataRangeAsync(dataInicio, dataFim);
            var apostasUsuario = apostas.Where(a => a.UsuarioId == usuarioId).ToList();

            var relatorio = new RelatorioComportamental
            {
                UsuarioId = usuarioId,
                DataInicio = dataInicio,
                DataFim = dataFim,
                TotalApostas = apostasUsuario.Count,
                ValorTotalApostado = apostasUsuario.Sum(a => a.Valor),
                ValorTotalGanho = apostasUsuario.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho.Value),
                ValorTotalPerdido = apostasUsuario.Where(a => a.Status == "Perdeu").Sum(a => a.Valor),
                DiasApostando = apostasUsuario.Select(a => a.DataAposta.Date).Distinct().Count(),
                ApostasConsecutivas = CalcularApostasConsecutivas(apostasUsuario),
                ApostasNoturnas = apostasUsuario.Count(a => a.DataAposta.Hour >= 22 || a.DataAposta.Hour < 6),
                MaiorAposta = apostasUsuario.Any() ? apostasUsuario.Max(a => a.Valor) : 0,
                MenorAposta = apostasUsuario.Any() ? apostasUsuario.Min(a => a.Valor) : 0,
                MediaApostas = apostasUsuario.Any() ? apostasUsuario.Average(a => a.Valor) : 0,
                PontuacaoRiscoInicial = usuario.PontuacaoRisco,
                PontuacaoRiscoFinal = await CalcularPontuacaoRiscoFinal(usuarioId, apostasUsuario),
                NivelRiscoInicial = usuario.NivelRisco,
                NivelRiscoFinal = await ClassificarNivelRiscoFinal(usuarioId, apostasUsuario),
                TotalIntervencoes = 0, // Implementar busca de intervenções
                IntervencoesAceitas = 0, // Implementar busca de intervenções aceitas
                DataGeracao = DateTime.Now
            };

            relatorio.GerarAnaliseComportamental();
            relatorio.Recomendacoes = await GerarRecomendacoesAsync(usuarioId);

            return relatorio;
        }

        public async Task<RelatorioComportamental> GerarRelatorioMensalAsync(int usuarioId, int mes, int ano)
        {
            var dataInicio = new DateTime(ano, mes, 1);
            var dataFim = dataInicio.AddMonths(1).AddDays(-1);
            return await GerarRelatorioAsync(usuarioId, dataInicio, dataFim);
        }

        public async Task<RelatorioComportamental> GerarRelatorioSemanalAsync(int usuarioId, DateTime dataInicio)
        {
            var dataFim = dataInicio.AddDays(7);
            return await GerarRelatorioAsync(usuarioId, dataInicio, dataFim);
        }

        public async Task<List<RelatorioComportamental>> ListarRelatoriosAsync(int usuarioId)
        {
            // Implementar busca no banco de dados
            return new List<RelatorioComportamental>();
        }

        public async Task<RelatorioComportamental> ObterRelatorioPorIdAsync(int id)
        {
            // Implementar busca no banco de dados
            return null;
        }

        public async Task<string> GerarAnaliseComportamentalAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return "Usuário não encontrado.";

            var analise = new System.Text.StringBuilder();
            
            analise.AppendLine($"=== ANÁLISE COMPORTAMENTAL - {usuario.Nome} ===");
            analise.AppendLine($"Data: {DateTime.Now:dd/MM/yyyy HH:mm}");
            analise.AppendLine();

            // Análise de risco
            analise.AppendLine("🔍 NÍVEL DE RISCO:");
            analise.AppendLine($"• Pontuação atual: {usuario.PontuacaoRisco} pontos");
            analise.AppendLine($"• Classificação: {usuario.ObterDescricaoRisco()}");
            analise.AppendLine();

            // Análise de apostas
            var apostas = await _apostaRepository.GetByUsuarioIdAsync(usuarioId);
            var apostasRecentes = apostas.Where(a => a.DataAposta >= DateTime.Now.AddDays(-30)).ToList();

            analise.AppendLine("📊 ANÁLISE DE APOSTAS (Últimos 30 dias):");
            analise.AppendLine($"• Total de apostas: {apostasRecentes.Count}");
            analise.AppendLine($"• Valor apostado: R$ {apostasRecentes.Sum(a => a.Valor):F2}");
            analise.AppendLine($"• Valor ganho: R$ {apostasRecentes.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho.Value):F2}");
            analise.AppendLine($"• Saldo líquido: R$ {apostasRecentes.Where(a => a.Status == "Ganhou" && a.ValorGanho.HasValue).Sum(a => a.ValorGanho.Value) - apostasRecentes.Sum(a => a.Valor):F2}");
            analise.AppendLine();

            // Padrões comportamentais
            analise.AppendLine("🎯 PADRÕES COMPORTAMENTAIS:");
            
            var apostasHoje = apostasRecentes.Count(a => a.DataAposta.Date == DateTime.Today);
            if (apostasHoje > 5)
            {
                analise.AppendLine("⚠️ ALERTA: Muitas apostas hoje (" + apostasHoje + ")");
            }

            var apostasNoturnas = apostasRecentes.Count(a => a.DataAposta.Hour >= 22 || a.DataAposta.Hour < 6);
            if (apostasNoturnas > 0)
            {
                analise.AppendLine("🌙 Apostas noturnas detectadas: " + apostasNoturnas);
            }

            var diasConsecutivos = CalcularDiasConsecutivosApostando(apostasRecentes);
            if (diasConsecutivos > 3)
            {
                analise.AppendLine("📅 Apostas consecutivas: " + diasConsecutivos + " dias");
            }

            analise.AppendLine();

            // Recomendações
            analise.AppendLine("💡 RECOMENDAÇÕES:");
            if (usuario.NivelRisco == NivelRisco.Alto)
            {
                analise.AppendLine("• URGENTE: Busque ajuda profissional imediatamente");
                analise.AppendLine("• Considere uma pausa completa nas apostas");
                analise.AppendLine("• Entre em contato com grupos de apoio");
            }
            else if (usuario.NivelRisco == NivelRisco.Medio)
            {
                analise.AppendLine("• Estabeleça limites rígidos de apostas");
                analise.AppendLine("• Pratique atividades alternativas");
                analise.AppendLine("• Monitore seu comportamento regularmente");
            }
            else
            {
                analise.AppendLine("• Continue apostando com responsabilidade");
                analise.AppendLine("• Mantenha os limites estabelecidos");
                analise.AppendLine("• Pratique atividades alternativas");
            }

            return analise.ToString();
        }

        public async Task<string> GerarRecomendacoesAsync(int usuarioId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return "Usuário não encontrado.";

            var recomendacoes = new System.Text.StringBuilder();
            
            recomendacoes.AppendLine("=== RECOMENDAÇÕES PERSONALIZADAS ===");
            recomendacoes.AppendLine();

            // Recomendações baseadas no nível de risco
            switch (usuario.NivelRisco)
            {
                case NivelRisco.Alto:
                    recomendacoes.AppendLine("🚨 AÇÕES IMEDIATAS NECESSÁRIAS:");
                    recomendacoes.AppendLine("• Pare de apostar imediatamente");
                    recomendacoes.AppendLine("• Busque ajuda profissional (psicólogo/psiquiatra)");
                    recomendacoes.AppendLine("• Entre em contato com grupos de apoio");
                    recomendacoes.AppendLine("• Informe familiares sobre a situação");
                    recomendacoes.AppendLine("• Considere bloqueio de acesso a sites de apostas");
                    break;

                case NivelRisco.Medio:
                    recomendacoes.AppendLine("⚠️ AÇÕES PREVENTIVAS RECOMENDADAS:");
                    recomendacoes.AppendLine("• Estabeleça limites rígidos de tempo e dinheiro");
                    recomendacoes.AppendLine("• Pratique atividades alternativas regularmente");
                    recomendacoes.AppendLine("• Monitore seu comportamento diariamente");
                    recomendacoes.AppendLine("• Evite apostar quando estiver estressado");
                    recomendacoes.AppendLine("• Considere buscar orientação profissional");
                    break;

                default:
                    recomendacoes.AppendLine("✅ MANTER COMPORTAMENTO RESPONSÁVEL:");
                    recomendacoes.AppendLine("• Continue apostando com moderação");
                    recomendacoes.AppendLine("• Mantenha os limites estabelecidos");
                    recomendacoes.AppendLine("• Pratique atividades alternativas");
                    recomendacoes.AppendLine("• Monitore seu comportamento");
                    break;
            }

            recomendacoes.AppendLine();
            recomendacoes.AppendLine("🎯 ATIVIDADES ALTERNATIVAS SUGERIDAS:");
            recomendacoes.AppendLine("• Exercícios físicos (caminhada, corrida, academia)");
            recomendacoes.AppendLine("• Hobbies criativos (pintura, música, artesanato)");
            recomendacoes.AppendLine("• Leitura e aprendizado");
            recomendacoes.AppendLine("• Socialização com amigos e família");
            recomendacoes.AppendLine("• Trabalho voluntário");

            return recomendacoes.ToString();
        }

        public async Task<decimal> CalcularSimulacaoInvestimentoAsync(int usuarioId, decimal valor, int meses)
        {
            // Simulação de investimento com rendimento de 6% ao ano
            var taxaAnual = 0.06m;
            var taxaMensal = taxaAnual / 12;
            
            var valorFinal = valor;
            for (int i = 0; i < meses; i++)
            {
                valorFinal += valorFinal * taxaMensal;
            }

            return valorFinal;
        }

        public async Task<List<string>> GerarAlertasConscientizacaoAsync(int usuarioId)
        {
            var alertas = new List<string>();

            alertas.Add("📊 ESTATÍSTICAS IMPORTANTES:");
            alertas.Add("• Apenas 1% dos apostadores são lucrativos a longo prazo");
            alertas.Add("• 90% das pessoas com problemas de apostas perdem dinheiro");
            alertas.Add("• Casas de apostas sempre têm vantagem matemática");

            alertas.Add("⚠️ SINAIS DE ALERTA:");
            alertas.Add("• Apostar mais do que pode perder");
            alertas.Add("• Tentar recuperar perdas com apostas maiores");
            alertas.Add("• Mentir sobre apostas para familiares");
            alertas.Add("• Apostar quando estressado ou triste");

            alertas.Add("💡 LEMBRETES IMPORTANTES:");
            alertas.Add("• Apostas são entretenimento, não investimento");
            alertas.Add("• Nunca aposte com dinheiro emprestado");
            alertas.Add("• Estabeleça limites e os respeite");
            alertas.Add("• Busque ajuda se sentir que perdeu o controle");

            return alertas;
        }

        public async Task<RelatorioComportamental> SalvarRelatorioAsync(RelatorioComportamental relatorio)
        {
            // Implementar persistência no banco de dados
            return relatorio;
        }

        private int CalcularApostasConsecutivas(List<Aposta> apostas)
        {
            var datasApostas = apostas.Select(a => a.DataAposta.Date).Distinct().OrderBy(d => d).ToList();
            
            if (datasApostas.Count < 2) return 0;

            int maxConsecutivas = 1;
            int consecutivas = 1;

            for (int i = 1; i < datasApostas.Count; i++)
            {
                if ((datasApostas[i] - datasApostas[i - 1]).Days == 1)
                {
                    consecutivas++;
                    maxConsecutivas = Math.Max(maxConsecutivas, consecutivas);
                }
                else
                {
                    consecutivas = 1;
                }
            }

            return maxConsecutivas;
        }

        private int CalcularDiasConsecutivosApostando(List<Aposta> apostas)
        {
            var datasApostas = apostas.Select(a => a.DataAposta.Date).Distinct().OrderByDescending(d => d).ToList();
            
            if (datasApostas.Count == 0) return 0;

            int diasConsecutivos = 1;
            var dataAtual = DateTime.Today;

            for (int i = 0; i < datasApostas.Count; i++)
            {
                if (datasApostas[i] == dataAtual.AddDays(-i))
                {
                    diasConsecutivos++;
                }
                else
                {
                    break;
                }
            }

            return diasConsecutivos - 1;
        }

        private async Task<int> CalcularPontuacaoRiscoFinal(int usuarioId, List<Aposta> apostas)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null) return 0;

            // Recalcular pontuação baseada nas apostas do período
            int pontuacao = 0;

            var apostasHoje = apostas.Count(a => a.DataAposta.Date == DateTime.Today);
            if (apostasHoje > 5) pontuacao += 20;

            var valorApostadoHoje = apostas.Where(a => a.DataAposta.Date == DateTime.Today).Sum(a => a.Valor);
            if (valorApostadoHoje > usuario.Saldo * 0.1m) pontuacao += 15;

            var diasConsecutivos = CalcularDiasConsecutivosApostando(apostas);
            if (diasConsecutivos > 3) pontuacao += 25;

            var apostasNoturnas = apostas.Count(a => a.DataAposta.Hour >= 22 || a.DataAposta.Hour < 6);
            if (apostasNoturnas > 0) pontuacao += 10;

            return pontuacao;
        }

        private async Task<NivelRisco> ClassificarNivelRiscoFinal(int usuarioId, List<Aposta> apostas)
        {
            var pontuacao = await CalcularPontuacaoRiscoFinal(usuarioId, apostas);

            if (pontuacao >= 70) return NivelRisco.Alto;
            if (pontuacao >= 40) return NivelRisco.Medio;
            return NivelRisco.Baixo;
        }
    }
}
