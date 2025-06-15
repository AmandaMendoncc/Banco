using Banco.Models.Core;
using Banco.Models.Identity;

namespace Banco.Data.Services
{
    public class LoanEvaluationService
    {
        private readonly ApplicationDbContext _context;

        public bool Avaliar(Emprestimo emprestimo, Cliente cliente)
        {
            var saldoTotal = _context.Contas.Where(c => c.ClienteId == cliente.Id).Sum(c => c.Saldo);
            if (saldoTotal < (emprestimo.ValorSolicitado * 0.20m))
            {
                return false;
            }

            bool temPendente = _context.Emprestimos
                .Any(e => e.ClienteId == cliente.Id && e.Status != StatusEmprestimo.Liquidado);

            if (temPendente) return false;

            return true;
        }
    }
}
