using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }
        
        public Roles Role { get; set; }

        public virtual Wallet? Wallet { get; set; }

        public virtual ICollection<Membership>? Memberships { get; set; }

        public virtual ICollection<UserAdvertismentView>? UserAdvertismentViews { get; set; }
        

        public User() 
        {

        }
    }

    public enum Roles
    {
        Admin = 1,
        User = 2
    }
}
