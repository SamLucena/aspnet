using System.ComponentModel.DataAnnotations;

namespace WellBooks.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Campo email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
