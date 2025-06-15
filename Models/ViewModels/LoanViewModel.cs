using System.ComponentModel.DataAnnotations;

namespace Banco.Models.ViewModels
{
    public class LoanViewModel
    {
        [Required]
        [Display(Name = "Valor Desejado")]
        [Range(100.0, 100000.0, ErrorMessage = "O valor deve ser entre R$100,00 e R$100.000,00")]
        public decimal ValorSolicitado { get; set; }

        [Required]
        [Display(Name = "Quantidade de Parcelas")]
        [Range(1, 48, ErrorMessage = "O número de parcelas deve ser entre 1 e 48.")]
        public int Parcelas { get; set; }
    }
}