using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EasyCash.Entities
{
    [Table("PaymentMethods")]
    public class PaymentMethod
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid UpdatedBy { get; set; }

        //[JsonIgnore]
        //public virtual ICollection<WalletPaymentMethod> WalletPaymentMethods { get; set; }

        public PaymentMethod()
        {

        }
    }
}
