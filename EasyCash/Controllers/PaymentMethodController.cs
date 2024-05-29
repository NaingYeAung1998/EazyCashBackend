using AutoMapper;
using EasyCash.BL;
using EasyCash.Entities;
using EasyCash.Models.Request;
using EasyCash.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EasyCash.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;
        //BL
        private readonly PaymentMethodBL _paymentMethodBL;

        public PaymentMethodController(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _paymentMethodBL = new PaymentMethodBL(_context, _config, _mapper);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaymentMethods([FromQuery]PaymentMethodPaginationRequestModel request)
        {
            ResponseModel response = await _paymentMethodBL.GetAllPaymentMethods(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod(PaymentMethodCreateRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
            
            if (currentUserID == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _paymentMethodBL.CreatePaymentMethod(request, Guid.Parse(currentUserID));

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePaymentMethod(PaymentMethodUpdateRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
            
            if (currentUserID == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _paymentMethodBL.UpdatePaymentMethod(request, Guid.Parse(currentUserID));

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeletePaymentMethod(Guid Id)
        {
            ResponseModel response = await _paymentMethodBL.DeletePaymentMethod(Id);
            return Ok(response);
        }

        private string? GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
