using System.ComponentModel.DataAnnotations;
using WellBooks.Models;

namespace WellBooks.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required(ErrorMessage = "Campo nome é obrigatório")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Campo preço é obrigatório")]
        public double Price { get; set; }
        [Required(ErrorMessage = "Campo descrição é obrigatório")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Campo da imagem é obrigatório")]
        public string ImgUrl { get; set; }
        [Required(ErrorMessage = "Campo de categoria é obrigatório")]
        public Category Category { get; set; }

    }
}
