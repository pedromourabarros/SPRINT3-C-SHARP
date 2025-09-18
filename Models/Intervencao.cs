using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa uma intervenção aplicada ao usuário
    /// </summary>
    public class Intervencao
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public TipoIntervencao Tipo { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public string AcaoRecomendada { get; set; } = string.Empty;
        public DateTime DataIntervencao { get; set; }
        public bool FoiVisualizada { get; set; } = false;
        public bool FoiAceita { get; set; } = false;
        public DateTime? DataVisualizacao { get; set; }
        public DateTime? DataAceitacao { get; set; }
        public int Prioridade { get; set; } = 1; // 1-5

        public Intervencao()
        {
            DataIntervencao = DateTime.Now;
        }

        public Intervencao(int usuarioId, TipoIntervencao tipo, string titulo, string mensagem, int prioridade = 1)
        {
            UsuarioId = usuarioId;
            Tipo = tipo;
            Titulo = titulo;
            Mensagem = mensagem;
            Prioridade = prioridade;
            DataIntervencao = DateTime.Now;
        }

        /// <summary>
        /// Marca a intervenção como visualizada
        /// </summary>
        public void MarcarComoVisualizada()
        {
            FoiVisualizada = true;
            DataVisualizacao = DateTime.Now;
        }

        /// <summary>
        /// Marca a intervenção como aceita
        /// </summary>
        public void MarcarComoAceita()
        {
            FoiAceita = true;
            DataAceitacao = DateTime.Now;
        }

        /// <summary>
        /// Obtém descrição da prioridade
        /// </summary>
        public string ObterDescricaoPrioridade()
        {
            return Prioridade switch
            {
                5 => "Urgente - Ação imediata",
                4 => "Alta - Próximas horas",
                3 => "Média - Próximo dia",
                2 => "Baixa - Próxima semana",
                1 => "Informativa - Quando possível",
                _ => "Não definida"
            };
        }

        public override string ToString()
        {
            return $"{DataIntervencao:dd/MM/yyyy HH:mm} | {Tipo} | {Titulo} | Prioridade: {Prioridade}";
        }
    }

    /// <summary>
    /// Tipos de intervenções disponíveis
    /// </summary>
    public enum TipoIntervencao
    {
        AlertaComportamento = 1,
        SugestaoAtividade = 2,
        LimiteApostas = 3,
        PausaObrigatoria = 4,
        ContatoApoio = 5,
        EducacaoRiscos = 6,
        SimulacaoInvestimento = 7,
        EstatisticasConscientizacao = 8,
        LembreteResponsabilidade = 9,
        ConviteTerapia = 10
    }
}
