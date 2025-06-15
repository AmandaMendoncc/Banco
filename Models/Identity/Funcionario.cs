
// Funcionario.cs
namespace Banco.Models.Identity
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Cargo { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}