using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Entidade que representa um usuário no sistema de análise comportamental
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        
        // Novos campos para detecção de compulsão
        public NivelRisco NivelRisco { get; set; } = NivelRisco.Baixo;
        public int PontuacaoRisco { get; set; } = 0;
        public DateTime UltimaAvaliacao { get; set; }
        public bool RecebeAlertas { get; set; } = true;
        public bool AceitaApoio { get; set; } = false;
        public string Telefone { get; set; } = string.Empty;
        public DateTime? DataUltimaAposta { get; set; }
        public int TotalApostasHoje { get; set; } = 0;
        public decimal ValorApostadoHoje { get; set; } = 0;
        public int DiasConsecutivosApostando { get; set; } = 0;
        
        // Campo para LGPD
        public bool ConsentimentoAceito { get; set; } = false;
        public DateTime? DataConsentimento { get; set; }

        public Usuario()
        {
            DataCadastro = DateTime.Now;
            Ativo = true;
            Saldo = 0;
            UltimaAvaliacao = DateTime.Now;
        }

        public Usuario(string nome, string email, decimal saldoInicial = 0)
        {
            Nome = nome;
            Email = email;
            Saldo = saldoInicial;
            DataCadastro = DateTime.Now;
            Ativo = true;
            UltimaAvaliacao = DateTime.Now;
        }

        /// <summary>
        /// Avalia o nível de risco do usuário baseado em padrões comportamentais
        /// </summary>
        public void CalcularPontuacaoRisco()
        {
            PontuacaoRisco = 0;
            
            // Fatores de risco
            if (TotalApostasHoje > 5) PontuacaoRisco += 20;
            if (ValorApostadoHoje > Saldo * 0.1m) PontuacaoRisco += 15;
            if (DiasConsecutivosApostando > 3) PontuacaoRisco += 25;
            if (DataUltimaAposta.HasValue && (DateTime.Now - DataUltimaAposta.Value).TotalHours < 2) PontuacaoRisco += 10;
            
            // Atualizar nível de risco
            if (PontuacaoRisco >= 70) NivelRisco = NivelRisco.Alto;
            else if (PontuacaoRisco >= 40) NivelRisco = NivelRisco.Medio;
            else NivelRisco = NivelRisco.Baixo;
            
            UltimaAvaliacao = DateTime.Now;
        }

        /// <summary>
        /// Verifica se o usuário está em risco de compulsão
        /// </summary>
        public bool EstaEmRisco()
        {
            return NivelRisco == NivelRisco.Alto || NivelRisco == NivelRisco.Medio;
        }

        /// <summary>
        /// Obtém descrição do nível de risco
        /// </summary>
        public string ObterDescricaoRisco()
        {
            return NivelRisco switch
            {
                NivelRisco.Baixo => "Baixo Risco - Comportamento controlado",
                NivelRisco.Medio => "Médio Risco - Atenção necessária",
                NivelRisco.Alto => "Alto Risco - Ajuda profissional recomendada",
                _ => "Não avaliado"
            };
        }

        public override string ToString()
        {
            return $"ID: {Id} | Nome: {Nome} | Email: {Email} | Saldo: R$ {Saldo:F2} | " +
                   $"Risco: {NivelRisco} ({PontuacaoRisco} pts) | Ativo: {(Ativo ? "Sim" : "Não")}";
        }
    }

    /// <summary>
    /// Enumeração para níveis de risco de compulsão
    /// </summary>
    public enum NivelRisco
    {
        Baixo = 0,
        Medio = 1,
        Alto = 2
    }
}
