using System;

namespace ApostasCompulsivas.Models
{
    /// <summary>
    /// Modelo que representa o histórico de operações do sistema
    /// </summary>
    public class Historico
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string TipoOperacao { get; set; } = string.Empty; // Deposito, Saque, Aposta, Ganho, Perda
        public decimal Valor { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime DataOperacao { get; set; }
        public decimal SaldoAnterior { get; set; }
        public decimal SaldoPosterior { get; set; }

        public Historico()
        {
            DataOperacao = DateTime.Now;
        }

        public Historico(int usuarioId, string tipoOperacao, decimal valor, string descricao, decimal saldoAnterior, decimal saldoPosterior)
        {
            UsuarioId = usuarioId;
            TipoOperacao = tipoOperacao;
            Valor = valor;
            Descricao = descricao;
            SaldoAnterior = saldoAnterior;
            SaldoPosterior = saldoPosterior;
            DataOperacao = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{DataOperacao:dd/MM/yyyy HH:mm} | {TipoOperacao} | R$ {Valor:F2} | " +
                   $"Saldo: {SaldoAnterior:F2} → {SaldoPosterior:F2} | {Descricao}";
        }
    }
}
