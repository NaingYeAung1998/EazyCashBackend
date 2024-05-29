using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyCash.Entities
{
    [Table("WalletPaymentMethods")]
    public class WalletPaymentMethod
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid WalletId { get; set;}
        [Required]
        public Guid PaymentMethodId { get; set; }
        [Required]
        public string AccountNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set;}

        //[ForeignKey("WalletId")]
        //public virtual Wallet Wallet { get; set; }

        //[ForeignKey("PaymentMethodId")]
        public virtual PaymentMethod PaymentMethod { get; set; }

        public WalletPaymentMethod()
        {

        }
    }
}
