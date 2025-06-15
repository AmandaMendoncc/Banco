using Banco.Data;
using Banco.Models.Core;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace Banco.Data.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GerarRelatorioTransacoesCSV(DateTime inicio, DateTime fim)
        {
            var transacoes = await _context.Transacoes
                .Where(t => t.Data.Date >= inicio.Date && t.Data.Date <= fim.Date)
                .OrderBy(t => t.Data)
                .ToListAsync();

            using var memoryStream = new MemoryStream();
            // Importante: Usar "leaveOpen: true" para que o stream não seja fechado pelo writer
            using var writer = new StreamWriter(memoryStream, System.Text.Encoding.UTF8, 1024, true);
            using var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("pt-BR"));

            csv.WriteRecords(transacoes);

            await writer.FlushAsync(); // Garante que tudo seja escrito no stream
            memoryStream.Position = 0; // Reseta a posição para o início para leitura

            return memoryStream.ToArray();
        }

        // A implementação para PDF com iTextSharp seria mais complexa,
        // mas o método precisa existir para evitar o erro de compilação.
        public async Task<byte[]> GerarRelatorioTransacoesPDF(DateTime inicio, DateTime fim)
        {
            // Lógica do PDF aqui...
            // Por enquanto, retornamos um array vazio para o código compilar.
            await Task.CompletedTask;
            return new byte[0];
        }
    }
}