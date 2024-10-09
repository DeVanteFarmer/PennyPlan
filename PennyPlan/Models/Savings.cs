using PennyPlan.Models;
using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Savings
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int SavingsAmount { get; set; } // This represents the user's monthly savings contribution

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

