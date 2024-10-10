using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PennyPlan.Models
{
    public class Bill
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string BillName { get; set; } // Acts as the category (e.g., Rent, Utilities, etc.)

        [Required]
        public int Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public bool Paid { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        [Column("updated_at")] // Maps to updated_at column in the database
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public User User { get; set; }

        public Category Category { get; set; }
    }
}
