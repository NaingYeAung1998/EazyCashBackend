using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCash.Entities
{
    [Table("Advertisments")]
    public class Advertisment
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string EmbedLink { get; set; }

        public decimal AmountPerWatch { get; set; }

        public AdvertismentTypes AdvertismentType { get; set; }

        public AdvertismentStatuses AdvertismentStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid CreatedBy { get; set; }

        public Guid UpdatedBy { get; set; }

        public virtual ICollection<UserAdvertismentView> UserAdvertismentViews { get; set; }

        public Advertisment()
        {

        }
    }

    public enum AdvertismentTypes
    {
        Facebook = 0,
        Youtube = 1,
        GoogleAds = 2
    }

    public enum AdvertismentStatuses
    {
        Active = 0,
        Inactive = 1,
    }
}
