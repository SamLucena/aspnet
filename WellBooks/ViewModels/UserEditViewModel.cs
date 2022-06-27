using System.ComponentModel.DataAnnotations;

namespace WellBooks.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
