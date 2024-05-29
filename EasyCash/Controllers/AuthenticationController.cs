using EasyCash.BL;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyCash.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationBL _authenticationBL;
        public AuthenticationController(ApplicationDbContext context, IConfiguration config)
        {
            _authenticationBL = new AuthenticationBL(context, config);
        }

        [HttpPost]
        public IActionResult Login(LoginRequestModel loginInfo)
        {
            ResponseModel response = _authenticationBL.Login(loginInfo);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult Register(RegisterWithMembershipRequestModel registerInfo)
        {
            ResponseModel response = _authenticationBL.Register(registerInfo);
            return Ok(response);
        }

        [HttpGet]

        public IActionResult GenerateWalletNumber()
        {
            string response = _authenticationBL.Create16DigitString();
            return Ok(response);
        }
    }
}
