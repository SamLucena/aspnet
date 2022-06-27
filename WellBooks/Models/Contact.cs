using System.ComponentModel.DataAnnotations;

namespace WellBooks.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo email é obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Campo da mensagem é obrigatório")]
        public string Message { get; set; }
    }
}
