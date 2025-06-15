using Banco.Models.Core;
using Banco.Models.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Banco.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Emprestimo> Emprestimos { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            builder.Entity<Conta>().Property(c => c.Saldo).HasPrecision(18, 2);
            builder.Entity<Transacao>().Property(t => t.Valor).HasPrecision(18, 2);
            builder.Entity<Emprestimo>().Property(e => e.ValorSolicitado).HasPrecision(18, 2);
            builder.Entity<Emprestimo>().Property(e => e.ValorAprovado).HasPrecision(18, 2);
            builder.Entity<Emprestimo>().Property(e => e.TaxaJurosAnual).HasPrecision(5, 2);
        }
    }
}