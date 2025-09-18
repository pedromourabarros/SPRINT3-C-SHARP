using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa uma aposta realizada por um usuário
    /// </summary>
    public class Aposta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TipoAposta { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Multiplicador { get; set; }
        public string Status { get; set; } = "Pendente"; // Pendente, Ganhou, Perdeu
        public decimal? ValorGanho { get; set; }
        public DateTime DataAposta { get; set; }
        public DateTime? DataResultado { get; set; }

        public Aposta()
        {
            DataAposta = DateTime.Now;
            Status = "Pendente";
        }

        public Aposta(int usuarioId, string tipoAposta, decimal valor, decimal multiplicador)
        {
            UsuarioId = usuarioId;
            TipoAposta = tipoAposta;
            Valor = valor;
            Multiplicador = multiplicador;
            DataAposta = DateTime.Now;
            Status = "Pendente";
        }

        /// <summary>
        /// Calcula o valor que seria ganho se a aposta for vencedora
        /// </summary>
        public decimal CalcularValorGanho()
        {
            return Valor * Multiplicador;
        }

        /// <summary>
        /// Finaliza a aposta com resultado
        /// </summary>
        public void FinalizarAposta(bool ganhou)
        {
            Status = ganhou ? "Ganhou" : "Perdeu";
            DataResultado = DateTime.Now;
            ValorGanho = ganhou ? CalcularValorGanho() : 0;
        }

        public override string ToString()
        {
            return $"ID: {Id} | Usuário: {UsuarioId} | Tipo: {TipoAposta} | Valor: R$ {Valor:F2} | " +
                   $"Multiplicador: {Multiplicador}x | Status: {Status} | Data: {DataAposta:dd/MM/yyyy HH:mm}";
        }
    }
}
