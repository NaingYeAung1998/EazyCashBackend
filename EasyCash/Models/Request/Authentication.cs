using System.ComponentModel.DataAnnotations;

namespace EasyCash.Models.Request
{
    public class LoginRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterRequestModel : LoginRequestModel
    {
        [Required]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
    }

    public class RegisterWithMembershipRequestModel : RegisterRequestModel
    {
        public DateTime? SubscriptionDate { get; set; } = DateTime.UtcNow;

        public decimal? SubscriptionAmount { get; set; }

        public int? TotalMonth { get; set; }
    }
}
