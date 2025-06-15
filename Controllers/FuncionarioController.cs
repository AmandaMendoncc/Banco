using Banco.Data;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Banco.Data.Services;

[Authorize(Roles = "Gerente,Admin")]
public class FuncionarioController : Controller
{
    private readonly ReportService _reportService;

    public FuncionarioController(ReportService reportService)
    {
        _reportService = reportService;
    }

    public IActionResult Relatorios() => View();

    [HttpPost]
    public async Task<IActionResult> ExportarCSV(DateTime inicio, DateTime fim)
    {
        var fileBytes = await _reportService.GerarRelatorioTransacoesCSV(inicio, fim);
        return File(fileBytes, "text/csv", $"relatorio_{DateTime.Now:yyyy-MM-dd}.csv");
    }

    [HttpPost]
    public async Task<IActionResult> ExportarPDF(DateTime inicio, DateTime fim)
    {
        var fileBytes = await _reportService.GerarRelatorioTransacoesPDF(inicio, fim);
        return File(fileBytes, "application/pdf", $"relatorio_{DateTime.Now:yyyy-MM-dd}.pdf");
    }
}