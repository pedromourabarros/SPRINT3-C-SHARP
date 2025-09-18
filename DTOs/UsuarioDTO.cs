using System;
using System.ComponentModel.DataAnnotations;

namespace ApostasCompulsivas.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal Saldo { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string NivelRisco { get; set; } = "Baixo";
        public int PontuacaoRisco { get; set; } = 0;
        public DateTime UltimaAvaliacao { get; set; }
        public bool RecebeAlertas { get; set; } = true;
        public bool AceitaApoio { get; set; } = false;
        public string Telefone { get; set; } = string.Empty;
        public DateTime? DataUltimaAposta { get; set; }
        public int TotalApostasHoje { get; set; } = 0;
        public decimal ValorApostadoHoje { get; set; } = 0;
        public int DiasConsecutivosApostando { get; set; } = 0;
        public bool ConsentimentoAceito { get; set; } = false;
        public DateTime? DataConsentimento { get; set; }
    }

    public class CriarUsuarioDTO
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Saldo inicial deve ser >= 0")]
        public decimal SaldoInicial { get; set; } = 0;

        [Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; } = string.Empty;

        [Required(ErrorMessage = "É necessário aceitar o consentimento")]
        public bool ConsentimentoAceito { get; set; } = false;
    }

    public class AtualizarUsuarioDTO
    {
        [Required(ErrorMessage = "Id é obrigatório")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone inválido")]
        public string Telefone { get; set; } = string.Empty;

        public bool RecebeAlertas { get; set; } = true;
        public bool AceitaApoio { get; set; } = false;
    }
}
