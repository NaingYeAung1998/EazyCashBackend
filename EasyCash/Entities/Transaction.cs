using Microsoft.Build.Execution;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid WalletId { get; set; }

        public Guid WalletPaymentMethodId { get; set; }

        public decimal Amount { get; set; }

        public TransactionTypes TransactionType { get; set; }

        public TransactionStatuses TransactionStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Wallet Wallet { get; set; }

        [ForeignKey("WalletPaymentMethodId")]
        public virtual WalletPaymentMethod WalletPaymentMethod { get; set; }

        public Transaction() 
        {

        }
    }

    public enum TransactionTypes
    {
        Deposit = 0,
        Withdraw = 1
    }

    public enum TransactionStatuses
    {
        Requested = 0,
        Approved = 1,
        Rejected = 2
    }
}
