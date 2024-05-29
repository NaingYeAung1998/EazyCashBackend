using AutoMapper;
using EasyCash.BL;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyCash.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;
        //BL
        private readonly AdvertismentBL _advertismentBL;
        private readonly UserBL _userBL;
        private readonly TransactionBL _transactionBL;

        public AdminController(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _advertismentBL = new AdvertismentBL(_context, _config, _mapper);
            _userBL = new UserBL(_context, _config, _mapper);
            _transactionBL = new TransactionBL(_context, _config, _mapper);
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList([FromQuery]UserPaginationRequestModel request)
        {
            ResponseModel response = await _userBL.GetUserList(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetTransactionList")]
        public async Task<IActionResult> GetTransactionList([FromQuery]TransactionPaginationRequestModel request)
        {
            ResponseModel response = await _transactionBL.GetTransactionList(request);
            return Ok(response);
        }
    }
}
