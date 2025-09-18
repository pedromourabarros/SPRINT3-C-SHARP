using System;
using System.ComponentModel.DataAnnotations;

namespace ApostasCompulsivas.DTOs
{
    public class HistoricoDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TipoOperacao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataOperacao { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoPosterior { get; set; }
    }

    public class CriarHistoricoDTO
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Tipo de operação é obrigatório")]
        public string TipoOperacao { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public decimal SaldoAnterior { get; set; }
        public decimal SaldoPosterior { get; set; }
    }
}
