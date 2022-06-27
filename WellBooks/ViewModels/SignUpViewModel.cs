using System.ComponentModel.DataAnnotations;
using WellBooks.Models;

namespace WellBooks.ViewModel
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo senha é obrigatório")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; } = "customer";

    }
}
