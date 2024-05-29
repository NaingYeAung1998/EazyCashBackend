using AutoMapper;
using EasyCash.BL;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyCash.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertismentController : ControllerBase
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;
        //BL
        private readonly AdvertismentBL _advertismentBL;

        public AdvertismentController(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _advertismentBL = new AdvertismentBL(_context, _config, _mapper);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdvertisments([FromQuery]AdvertismentPaginationRequestModel request)
        {
            ResponseModel response = await _advertismentBL.GetAllAdvertisments(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvertisment(AdvertismentCreateRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
           
            if(currentUserID == null) {
                return Unauthorized();
            }
            ResponseModel response = await _advertismentBL.CreateAdvertisment(request, Guid.Parse(currentUserID));
            
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAdvertisment(AdvertismentUpdateRequestModel request)
        {
            var currentUserID = GetCurrentUserId();
            
            if (currentUserID == null)
            {
                return Unauthorized();
            }
            ResponseModel response = await _advertismentBL.UpdateAdvertisment(request, Guid.Parse(currentUserID));

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAdvertisment(Guid Id)
        {
            ResponseModel response = await _advertismentBL.DeleteAdvertisment(Id);
            return Ok(response);
        }

        [Route("ChangeStatus")]
        [HttpPost]
        public async Task<IActionResult> ChangeStatus(AdvertismentChangeStatusRequestModel request)
        {
            ResponseModel response = await _advertismentBL.ChangeAdvertismentStatus(request);
            return Ok(response);
        }

        private string? GetCurrentUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
