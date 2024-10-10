using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsBillCategory { get; set; } = false;

        public bool IsTransactionCategory { get; set; } = false;
    }
}
