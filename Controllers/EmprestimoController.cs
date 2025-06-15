using Banco.Data;
using Banco.Data.Services;
using Banco.Models.Core;
using Banco.Models.Identity;
using Banco.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Authorize]
public class EmprestimoController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LoanEvaluationService _loanService;

    [HttpGet]
    public IActionResult Solicitar() => View();

    [HttpPost]
    public async Task<IActionResult> Solicitar(LoanViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.UserId == user.Id);

        var emprestimo = new Emprestimo { /* MODELLL OREENCHER */ };

        bool aprovado = _loanService.Avaliar(emprestimo, cliente);
        emprestimo.Status = aprovado ? StatusEmprestimo.Aprovado : StatusEmprestimo.Rejeitado;

        _context.Emprestimos.Add(emprestimo);
        await _context.SaveChangesAsync();

        return View("SolicitacaoResultado", emprestimo);
    }
}