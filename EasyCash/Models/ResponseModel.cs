namespace EasyCash.Models
{
    public class ResponseModel
    {
        public ResponseStatusCodes Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }

    public enum ResponseStatusCodes
    {
        Success = 0,
        Fail =1
    }
}
