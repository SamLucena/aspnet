using System.ComponentModel.DataAnnotations;
using WellBooks.Models;

namespace WellBooks.ViewModel
{
    public class SignUpViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string Role { get; set; } = "customer";

    }
}
