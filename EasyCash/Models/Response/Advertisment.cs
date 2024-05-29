using EasyCash.Entities;

namespace EasyCash.Models.Response
{
    public class AdvertismentResponseModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string EmbedLink { get; set; }

        public decimal AmountPerWatch { get; set; }

        public AdvertismentTypes AdvertismentType { get; set; }

        public AdvertismentStatuses AdvertismentStatus { get; set; }

        public DateTime CreatedAt { get; set; }

    }

    public class UserAvailableAdvertismentResponseModel : AdvertismentResponseModel
    {
        public bool IsAvailable { get; set; }

        public DateTime? LastWatchedAt { get; set; }
    }
}
