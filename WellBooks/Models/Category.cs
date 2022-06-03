using System.ComponentModel.DataAnnotations;

namespace WellBooks.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(5)]
        public string Name { get; set; }
    }
}
