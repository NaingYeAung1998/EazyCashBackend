using EasyCash.Entities;

namespace EasyCash.Models.Request
{
    public class UserPaginationRequestModel
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        public string? Search { get; set; } = string.Empty;
    }
}
