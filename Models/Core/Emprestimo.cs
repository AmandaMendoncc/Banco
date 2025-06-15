// Emprestimo.cs
using Banco.Models.Identity;

namespace Banco.Models.Core
{
    public enum StatusEmprestimo { Solicitado, Aprovado, Rejeitado, Liquidado }
    public class Emprestimo
    {
        public int Id { get; set; }
        public decimal ValorSolicitado { get; set; }
        public decimal ValorAprovado { get; set; }
        public decimal TaxaJurosAnual { get; set; }
        public int Parcelas { get; set; }
        public StatusEmprestimo Status { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}