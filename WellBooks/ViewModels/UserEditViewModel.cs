using System.ComponentModel.DataAnnotations;

namespace WellBooks.ViewModels
{
    public class UserEditViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
