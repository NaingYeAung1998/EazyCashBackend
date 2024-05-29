using EasyCash.Entities;

namespace EasyCash.Models.Request
{
    public class AdvertismentPaginationRequestModel
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        public string? Search { get; set; } = string.Empty;

        public AdvertismentStatuses? Status { get; set; }
    }
    public class AdvertismentCreateRequestModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string EmbedLink { get; set; }

        public decimal AmountPerWatch { get; set; }

        public AdvertismentTypes AdvertismentType { get; set; }

        public AdvertismentStatuses AdvertismentStatus { get; set; }
    }

    public class AdvertismentUpdateRequestModel : AdvertismentCreateRequestModel
    {
        public Guid Id { get; set; }
    }

    public class AdvertismentChangeStatusRequestModel
    {
        public Guid Id { get; set; }

        public AdvertismentStatuses AdvertismentStatus { get; set; }
    }
}
