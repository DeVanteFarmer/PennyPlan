using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Budget
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TotalIncome { get; set; }

        [Required]
        public int TotalBills { get; set; }

        [Required]
        public int DailySpendingLimit { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
