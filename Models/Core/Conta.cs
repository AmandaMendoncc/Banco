// Conta.cs
using Banco.Models.Identity;

namespace Banco.Models.Core
{
    public enum TipoConta { Corrente, Poupanca, Investimento }
    public class Conta
    {
        public int Id { get; set; }
        public string NumeroConta { get; set; }
        public string Agencia { get; set; }
        public TipoConta Tipo { get; set; }
        public decimal Saldo { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}