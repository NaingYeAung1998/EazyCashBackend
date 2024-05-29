namespace EasyCash.Models.Request
{
    public class UserAdvertismentViewCreateRequestModel
    {
        public Guid AdvertismentId {get;set;}
        
        public decimal EarnedAmount { get; set; }
    }
}
