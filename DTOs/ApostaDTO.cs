using System;
using System.ComponentModel.DataAnnotations;

namespace ApostasCompulsivas.DTOs
{
    public class ApostaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TipoAposta { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Multiplicador { get; set; }
        public string Status { get; set; } = "Pendente";
        public decimal? ValorGanho { get; set; }
        public DateTime DataAposta { get; set; }
        public DateTime? DataResultado { get; set; }
    }

    public class CriarApostaDTO
    {
        [Required(ErrorMessage = "Usuário é obrigatório")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Tipo de aposta é obrigatório")]
        public string TipoAposta { get; set; } = string.Empty;

        [Range(1, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Multiplicador deve ser maior que zero")]
        public decimal Multiplicador { get; set; }
    }

    public class FinalizarApostaDTO
    {
        [Required(ErrorMessage = "ApostaId é obrigatório")]
        public int ApostaId { get; set; }

        [Required(ErrorMessage = "Campo Ganhou é obrigatório")]
        public bool Ganhou { get; set; }
    }
}
