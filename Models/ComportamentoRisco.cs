using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa um comportamento de risco identificado
    /// </summary>
    public class ComportamentoRisco
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public TipoComportamento Tipo { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Severidade { get; set; } // 1-10
        public DateTime DataDetectado { get; set; }
        public bool FoiNotificado { get; set; } = false;
        public string AcaoRecomendada { get; set; } = string.Empty;
        public bool AcaoExecutada { get; set; } = false;

        public ComportamentoRisco()
        {
            DataDetectado = DateTime.Now;
        }

        public ComportamentoRisco(int usuarioId, TipoComportamento tipo, string descricao, int severidade)
        {
            UsuarioId = usuarioId;
            Tipo = tipo;
            Descricao = descricao;
            Severidade = severidade;
            DataDetectado = DateTime.Now;
        }

        /// <summary>
        /// Obtém descrição da severidade
        /// </summary>
        public string ObterDescricaoSeveridade()
        {
            return Severidade switch
            {
                >= 8 => "Crítico - Ação imediata necessária",
                >= 6 => "Alto - Intervenção recomendada",
                >= 4 => "Médio - Monitoramento necessário",
                >= 2 => "Baixo - Atenção preventiva",
                _ => "Mínimo - Observação"
            };
        }

        public override string ToString()
        {
            return $"{DataDetectado:dd/MM/yyyy HH:mm} | {Tipo} | Severidade: {Severidade}/10 | {Descricao}";
        }
    }

    /// <summary>
    /// Tipos de comportamentos de risco
    /// </summary>
    public enum TipoComportamento
    {
        ApostasFrequentes = 1,
        ValoresAltos = 2,
        ApostasConsecutivas = 3,
        ApostasNoturnas = 4,
        PerdasRecorrentes = 5,
        TentativaRecuperacao = 6,
        ApostasEmocionais = 7,
        NegligenciarResponsabilidades = 8,
        MentirSobreApostas = 9,
        ApostasComDinheiroEmprestado = 10
    }
}
