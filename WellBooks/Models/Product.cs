using System.ComponentModel.DataAnnotations;

namespace WellBooks.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; } 
        public string ImgUrl { get; set; }
        public Category Category { get; set; }
    }
}
