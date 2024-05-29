using EasyCash.Entities;

namespace EasyCash.Models.Response
{
    public class UserProfileResponseModel
    {
        public Guid UserId { get; set; }

        public string Name { get; set; }

        public List<UserAvailableAdvertismentResponseModel> Advertisments { get; set;}
    }

    public class UserListResponseModel
    {
        public Guid Id { get;  set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public DateTime RenewalDate { get; set; }

        public int TotalWatchedAdvertisments { get; set; }
    }
}
