using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("updated_at")] // Maps to updated_at column in the database
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
