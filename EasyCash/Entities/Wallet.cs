using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("Wallets")]
    public class Wallet
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string WalletNumber { get; set; }

        public Guid UserId { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<WalletPaymentMethod> WalletPaymentMethods { get; set; }

        public virtual ICollection<Transaction> WalletTransactions { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public Wallet()
        {

        }
    }
}
