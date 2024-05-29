using EasyCash.Entities;
using EasyCash.Migrations;

namespace EasyCash.Models.Response
{
    public class WalletBalanceResponseModel
    {
        public Guid Id { get; set; }

        public string WalletNumber { get; set; }

        public decimal Balance { get; set; }
    }

    public class WalletInfoResponseModel : WalletBalanceResponseModel
    {
        public string Name { get; set; }

        public DateTime ExpiryDate { get; set;}

        public ICollection<Transaction> WalletTransactions { get; set; }

        public ICollection<WalletPaymentMethod> WalletPaymentMethods { get; set; }
    }

    public class WithdrawRequestModel
    {
        public Guid PaymentMethodId { get; set; }

        public decimal Amount { get; set; }
    }
}
