using AutoMapper;
using EasyCash.BL;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Completion;
using System.Security.Claims;

namespace EasyCash.Controllers
{
    [Authorize(Roles = "User", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;
        //BL
        private readonly AdvertismentBL _advertismentBL;
        private readonly WalletBL _walletBL;
        private readonly PaymentMethodBL _paymentMethodBL;
        private readonly UserBL _userBL;

        public UserController(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _advertismentBL = new AdvertismentBL(_context, _config, _mapper);
            _walletBL = new WalletBL(_context, _config, _mapper);
            _paymentMethodBL = new PaymentMethodBL(_context, _config, _mapper);
            _userBL = new UserBL(_context, _config, _mapper);
        }

        [HttpPost]
        [Route("Watch/WatchAdvertisment")]
        public async Task<IActionResult> WatchAdvertisment(UserAdvertismentViewCreateRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
            
            if (currentUserID == null)
            {
                return Unauthorized();
            }

            ResponseModel response = await _advertismentBL.WatchAdvertisment(Guid.Parse(currentUserID), request);
            return Ok(response);
        }

        [HttpGet]
        [Route("Watch/GetAvailableAdvertisments")]
        public async Task<IActionResult> GetAvailableAdvertisments([FromQuery]AdvertismentPaginationRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
            
            if (currentUserID == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _advertismentBL.GetUserAvailableAdvertisments(Guid.Parse(currentUserID), request);
            return Ok(response);
        }

        [HttpGet]
        [Route("Wallet/GetWalletBalance")]
        public async Task<IActionResult> GetWalletBalance()
        {
            var currentWalletId = GetCurrentWalletId();
            if (currentWalletId == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _walletBL.GetWalletBalance(Guid.Parse(currentWalletId));
            return Ok(response);
        }

        [HttpGet]
        [Route("Wallet/GetWalletInfo")]
        public async Task<IActionResult> GetWalletInfo()
        {
            var currentWalletId = GetCurrentWalletId();
            if (currentWalletId == null)
            {
                return Unauthorized();
            }

            ResponseModel response = await _walletBL.GetWalletInfo(Guid.Parse(currentWalletId));
            return Ok(response);
        }

        [HttpGet]
        [Route("Wallet/GetWalletPaymentMethods")]
        public async Task<IActionResult> GetWalletPaymentMethods()
        {
            var currentWalletId = GetCurrentWalletId();
            if(currentWalletId == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _walletBL.GetWalletPaymentMethods(Guid.Parse(currentWalletId));
            return Ok(response);
        }

        [HttpPost]
        [Route("Wallet/AddWalletPaymentMethod")]
        public async Task<IActionResult> AddWalletPaymentMethod(WalletPaymentMethodCreateRequestModel request)
        {
            var currentWalletId = GetCurrentWalletId();
            if (currentWalletId == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _walletBL.AddWalletPaymentMethod(request, Guid.Parse(currentWalletId));
            return Ok(response);
        }

        [HttpGet]
        [Route("Payment/GetAvailablePaymentMethods")]
        public async Task<IActionResult> GetPaymentMethods([FromQuery]PaymentMethodPaginationRequestModel request)
        {
            ResponseModel response = await _paymentMethodBL.GetAllPaymentMethods(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetProfile")]
        public async Task<IActionResult> GetProfile()
        {
            var currentUserId = GetCurrentUserId();
            ResponseModel response = await _userBL.GetUserProfile(Guid.Parse(currentUserId));
            return Ok(response);
        }

        [HttpPost]
        [Route("Wallet/Withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawRequestModel request)
        {
            var currentWalletId = GetCurrentWalletId();
            ResponseModel response = await _walletBL.Withdraw(request, Guid.Parse(currentWalletId));
            return Ok(response);
        }

        [HttpGet]
        [Route("Wallet/GetWalletTransactions")]
        public async Task<IActionResult> GetWalletTransactions()
        {
            var currentWalletId = GetCurrentWalletId();
            ResponseModel response = await _walletBL.GetWalletTransactions(Guid.Parse(currentWalletId));
            return Ok(response);
        }

        private string? GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        private string? GetCurrentWalletId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;
        }
    }
}
