using Banco.Models.Identity;
using Banco.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            if (!string.IsNullOrEmpty(user.OtpSecretKey))
            {
                await _signInManager.SignOutAsync();
                await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
                return RedirectToAction("VerifyOtp", new { email = user.Email });
            }

            await _signInManager.SignInAsync(user, model.RememberMe);
            return RedirectToAction("Index", "Home");
        }
        ModelState.AddModelError(string.Empty, "Login ou senha inválidos.");
        return View(model);
    }

    [HttpGet]
    public IActionResult VerifyOtp(string email) => View(new OtpViewModel { Email = email });

    [HttpPost]
    public async Task<IActionResult> VerifyOtp(OtpViewModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var totp = new Totp(Base32Encoding.ToBytes(user.OtpSecretKey));

        if (totp.VerifyTotp(model.OtpCode, out long timeWindowUsed, new VerificationWindow(2, 2)))
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Dashboard", "Cliente"); 
        }

        ModelState.AddModelError("OtpCode", "Código OTP inválido.");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, NomeCompleto = model.NomeCompleto };

            var secretKey = KeyGeneration.GenerateRandomKey(20);
            user.OtpSecretKey = Base32Encoding.ToString(secretKey);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                ViewBag.OtpSecretKey = user.OtpSecretKey;
                ViewBag.QrCodeUri = $"otpauth://totp/BancoApp:{user.Email}?secret={user.OtpSecretKey}&issuer=BancoApp";
                return View("RegistrationSuccess");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}