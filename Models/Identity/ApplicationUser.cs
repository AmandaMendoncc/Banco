using Microsoft.AspNetCore.Identity;

namespace Banco.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; }
        public string? OtpSecretKey { get; set; }
    }
}