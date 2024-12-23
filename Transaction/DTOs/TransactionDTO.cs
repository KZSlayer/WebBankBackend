using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class TransactionDTO
    {
        public int Id { get; set; }
        public long? FromAccountNumber { get; set; }
        public long? ToAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
