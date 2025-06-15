using Banco.Data;
using Banco.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ClienteController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ClienteController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var cliente = await _context.Clientes
                                    .Include(c => c.Contas)
                                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

        if (cliente == null) return Unauthorized();

        return View(cliente.Contas);
    }

    [HttpPost]
    public async Task<IActionResult> Depositar(int contaId, decimal valor)
    {
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    [HttpPost]
    public async Task<IActionResult> Sacar(int contaId, decimal valor)
    {
        await _context.SaveChangesAsync();
        return RedirectToAction("Dashboard");
    }

    // TODO: implementar Transferir, ConsultarSaldo etc.
}
