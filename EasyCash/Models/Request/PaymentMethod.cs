namespace EasyCash.Models.Request
{
    public class PaymentMethodPaginationRequestModel
    {
        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;

        public string? Search { get; set; } = string.Empty;
    }
    public class PaymentMethodCreateRequestModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class PaymentMethodUpdateRequestModel : PaymentMethodCreateRequestModel
    {
        public Guid Id { get; set; }
    }
}
