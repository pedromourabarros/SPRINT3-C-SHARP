using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa um relatório comportamental do usuário
    /// </summary>
    public class RelatorioComportamental
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int TotalApostas { get; set; }
        public decimal ValorTotalApostado { get; set; }
        public decimal ValorTotalGanho { get; set; }
        public decimal ValorTotalPerdido { get; set; }
        public int DiasApostando { get; set; }
        public int ApostasConsecutivas { get; set; }
        public int ApostasNoturnas { get; set; }
        public decimal MaiorAposta { get; set; }
        public decimal MenorAposta { get; set; }
        public decimal MediaApostas { get; set; }
        public int PontuacaoRiscoInicial { get; set; }
        public int PontuacaoRiscoFinal { get; set; }
        public NivelRisco NivelRiscoInicial { get; set; }
        public NivelRisco NivelRiscoFinal { get; set; }
        public int TotalIntervencoes { get; set; }
        public int IntervencoesAceitas { get; set; }
        public string AnaliseComportamental { get; set; } = string.Empty;
        public string Recomendacoes { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; }

        public RelatorioComportamental()
        {
            DataGeracao = DateTime.Now;
        }

        /// <summary>
        /// Calcula o saldo líquido do período
        /// </summary>
        public decimal CalcularSaldoLiquido()
        {
            return ValorTotalGanho - ValorTotalPerdido;
        }

        /// <summary>
        /// Calcula a taxa de vitória
        /// </summary>
        public decimal CalcularTaxaVitoria()
        {
            if (TotalApostas == 0) return 0;
            return (decimal)ValorTotalGanho / ValorTotalApostado * 100;
        }

        /// <summary>
        /// Calcula a taxa de aceitação de intervenções
        /// </summary>
        public decimal CalcularTaxaAceitacaoIntervencoes()
        {
            if (TotalIntervencoes == 0) return 0;
            return (decimal)IntervencoesAceitas / TotalIntervencoes * 100;
        }

        /// <summary>
        /// Verifica se houve melhora no comportamento
        /// </summary>
        public bool HouveMelhora()
        {
            return PontuacaoRiscoFinal < PontuacaoRiscoInicial;
        }

        /// <summary>
        /// Obtém classificação do comportamento
        /// </summary>
        public string ObterClassificacaoComportamento()
        {
            if (PontuacaoRiscoFinal >= 70) return "Comportamento de Alto Risco";
            if (PontuacaoRiscoFinal >= 40) return "Comportamento de Médio Risco";
            if (PontuacaoRiscoFinal >= 20) return "Comportamento de Baixo Risco";
            return "Comportamento Controlado";
        }

        /// <summary>
        /// Gera análise comportamental automática
        /// </summary>
        public void GerarAnaliseComportamental()
        {
            var analise = new System.Text.StringBuilder();
            
            analise.AppendLine($"Período analisado: {DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy}");
            analise.AppendLine($"Total de apostas: {TotalApostas}");
            analise.AppendLine($"Valor total apostado: R$ {ValorTotalApostado:F2}");
            analise.AppendLine($"Saldo líquido: R$ {CalcularSaldoLiquido():F2}");
            analise.AppendLine($"Taxa de vitória: {CalcularTaxaVitoria():F1}%");
            analise.AppendLine($"Dias apostando: {DiasApostando}");
            analise.AppendLine($"Apostas consecutivas: {ApostasConsecutivas}");
            analise.AppendLine($"Apostas noturnas: {ApostasNoturnas}");
            analise.AppendLine($"Nível de risco: {NivelRiscoFinal} ({PontuacaoRiscoFinal} pontos)");
            analise.AppendLine($"Intervenções aceitas: {CalcularTaxaAceitacaoIntervencoes():F1}%");
            
            if (HouveMelhora())
            {
                analise.AppendLine("✅ Melhora no comportamento detectada!");
            }
            else if (PontuacaoRiscoFinal > PontuacaoRiscoInicial)
            {
                analise.AppendLine("⚠️ Piora no comportamento detectada!");
            }
            
            AnaliseComportamental = analise.ToString();
        }

        public override string ToString()
        {
            return $"Relatório {DataInicio:dd/MM/yyyy} a {DataFim:dd/MM/yyyy} - " +
                   $"{TotalApostas} apostas - R$ {ValorTotalApostado:F2} - Risco: {NivelRiscoFinal}";
        }
    }
}
