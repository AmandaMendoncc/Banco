using Banco.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SkiaSharp;
using System.Text.Json;

namespace Banco.Data.Services
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            LogAuditoria(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            LogAuditoria(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void LogAuditoria(DbContext context)
        {
            context.ChangeTracker.DetectChanges();
            var logs = new List<Auditoria>();

            foreach (var entry in context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted))
            {
                if (entry.Entity is Auditoria) continue;

                var log = new Auditoria
                {
                    TabelaAfetada = entry.Metadata.GetTableName(),
                    Acao = entry.State.ToString(),
                    DataHora = DateTime.UtcNow,
                    Detalhes = GetChangeDetails(entry)
                };

                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
                if (primaryKey != null)
                {
                    log.RegistroId = Convert.ToInt32(primaryKey.CurrentValue);
                }

                logs.Add(log);
            }

            if (logs.Any())
            {
                context.Set<Auditoria>().AddRange(logs);
            }
        }

        private string GetChangeDetails(EntityEntry entry)
        {
            var details = new Dictionary<string, object>();
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                details["NovosValores"] = entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
            }
            if (entry.State == EntityState.Deleted || entry.State == EntityState.Modified)
            {
                details["ValoresAntigos"] = entry.Properties.ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
            }
            return JsonSerializer.Serialize(details);
        }
    }
}