using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa uma atividade alternativa sugerida
    /// </summary>
    public class AtividadeAlternativa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public CategoriaAtividade Categoria { get; set; }
        public int DuracaoMinutos { get; set; }
        public decimal CustoAproximado { get; set; }
        public string Localizacao { get; set; } = string.Empty;
        public bool RequerEquipamento { get; set; } = false;
        public string EquipamentoNecessario { get; set; } = string.Empty;
        public int NivelDificuldade { get; set; } = 1; // 1-5
        public string Beneficios { get; set; } = string.Empty;
        public bool Ativa { get; set; } = true;

        public AtividadeAlternativa()
        {
        }

        public AtividadeAlternativa(string nome, string descricao, CategoriaAtividade categoria, int duracaoMinutos)
        {
            Nome = nome;
            Descricao = descricao;
            Categoria = categoria;
            DuracaoMinutos = duracaoMinutos;
        }

        /// <summary>
        /// Obtém descrição do nível de dificuldade
        /// </summary>
        public string ObterDescricaoDificuldade()
        {
            return NivelDificuldade switch
            {
                1 => "Iniciante - Fácil",
                2 => "Básico - Simples",
                3 => "Intermediário - Moderado",
                4 => "Avançado - Desafiador",
                5 => "Expert - Complexo",
                _ => "Não definido"
            };
        }

        /// <summary>
        /// Obtém descrição do custo
        /// </summary>
        public string ObterDescricaoCusto()
        {
            return CustoAproximado switch
            {
                0 => "Gratuito",
                < 10 => "Muito barato",
                < 50 => "Barato",
                < 100 => "Moderado",
                < 200 => "Caro",
                _ => "Muito caro"
            };
        }

        public override string ToString()
        {
            return $"{Nome} - {Categoria} - {DuracaoMinutos}min - {ObterDescricaoCusto()}";
        }
    }

    /// <summary>
    /// Categorias de atividades alternativas
    /// </summary>
    public enum CategoriaAtividade
    {
        Esportes = 1,
        Artes = 2,
        Educacao = 3,
        Social = 4,
        Relaxamento = 5,
        Voluntariado = 6,
        Hobbies = 7,
        Exercicio = 8,
        Leitura = 9,
        Musica = 10
    }
}
