// Transacao.cs
namespace Banco.Models.Core
{
    public enum TipoTransacao { Deposito, Saque, TransferenciaEntrada, TransferenciaSaida }
    public class Transacao
    {
        public int Id { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public TipoTransacao Tipo { get; set; }
        public string Descricao { get; set; }
        public int ContaId { get; set; }
        public Conta Conta { get; set; }
    }
}