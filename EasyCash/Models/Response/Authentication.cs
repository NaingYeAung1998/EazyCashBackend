namespace EasyCash.Models.Response
{
    public class LoginResponseModel
    {
        public AuthStatusCodes StatusCode { get; set; }
        public string Message { get; set; }
        public object Token { get; set; }
        public string Role { get; set; }
    }

    public class RegisterResponseModel : LoginResponseModel
    {

    }

    public enum AuthStatusCodes
    {
        Success = 0,
        IncorrectUsernameOrPassword = 1,
        UnavailableUsername = 2,
        UnavailablePassword = 3,
        PasswordsNotMatch = 4,
        UserNotFound = 5
    }
}
