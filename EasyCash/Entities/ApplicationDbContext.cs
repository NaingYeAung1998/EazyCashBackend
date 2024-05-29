using Microsoft.EntityFrameworkCore;

namespace EasyCash.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Advertisment> Advertisments { get; set;}

        public DbSet<UserAdvertismentView> UserAdvertismentViews { get; set; } 
        
        public DbSet<Membership> Memberships { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<WalletPaymentMethod> WalletPaymentMethods { get; set; }

    }
}
