namespace EasyCash.Models.Response
{
    public class PaginatedList<T>
    {
        public int Page { get; set; }

        public int PerPage { get; set; }

        public int Total { get; set; }

        public int TotalPages { get; set; }

        public List<T> Data { get; set; }
    }
}
