using System.ComponentModel.DataAnnotations;

namespace Banco.Models.ViewModels
{
    public class OtpViewModel
    {
        public string Email { get; set; }

        [Required(ErrorMessage = "O código OTP é obrigatório.")]
        [Display(Name = "Código de Verificação")]
        public string OtpCode { get; set; }
    }
}