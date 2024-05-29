using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("Memberships")]
    public class Membership
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime SubscriptionDate { get; set; }

        public decimal SubscriptionAmount { get; set; }

        public DateTime RenewalDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public Membership()
        {

        }
    }
}
