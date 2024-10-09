using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string BillName { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public bool Paid { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public Category? Category { get; set; }

        public User User { get; set; }
    }
}
