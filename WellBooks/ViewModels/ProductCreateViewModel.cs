using System.ComponentModel.DataAnnotations;
using WellBooks.Models;

namespace WellBooks.ViewModels
{
    public class ProductCreateViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public Category Category { get; set; }

    }
}
