using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.MVC.Data
{
    public class BlogCategory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Blog")]
        [Required]
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }

        
        [ForeignKey("Category")]
        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
