using System.ComponentModel.DataAnnotations;

namespace PennyPlan.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string TransactionName { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
