using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("UserAdvertismentViews")]
    public class UserAdvertismentView
    {
        [Key]
        [Required]
        public Guid Id { get; set; }
        
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid AdvertismentId { get; set; }

        public decimal EarnedAmount { get; set; }

        public DateTime WatchDate { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("AdvertismentId")]
        public virtual Advertisment Advertisment { get; set; }

        public UserAdvertismentView()
        {

        }
    }
}
