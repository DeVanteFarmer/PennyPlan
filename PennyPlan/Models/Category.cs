using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Category
    {
            public int Id { get; set; }

            [Required]
            [MaxLength(255)]
            public string Name { get; set; }

            [MaxLength(255)]
            public string Description { get; set; }
        
    }
}
