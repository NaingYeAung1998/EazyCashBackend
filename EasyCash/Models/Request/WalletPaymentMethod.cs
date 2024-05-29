using System.ComponentModel.DataAnnotations;

namespace EasyCash.Models.Request
{
    public class WalletPaymentMethodCreateRequestModel
    {
        public Guid PaymentMethodId { get; set; }
        [Required]
        public string AccountNumber { get; set; }
    }
}
