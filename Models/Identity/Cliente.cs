// Cliente.cs
using Banco.Models.Core;

namespace Banco.Models.Identity
{
    public class Cliente
    {
        public int Id { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<Conta> Contas { get; set; }
    }
}