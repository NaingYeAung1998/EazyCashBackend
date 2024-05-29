using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EasyCash.BL
{
    public class UserBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;

        private readonly int _adRenewHour;

        public UserBL(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _adRenewHour = int.Parse(_config["AdRenewHour"]);
        }

        public async Task<ResponseModel> GetUserProfile(Guid userId)
        {
            ResponseModel response = new ResponseModel();
            var watchedAds = await _context.UserAdvertismentViews.Where(x => x.UserId == userId).OrderByDescending(x => x.WatchDate).Take(3).Include(x => x.User).Include(x => x.Advertisment).ToListAsync();

            UserProfileResponseModel model = new UserProfileResponseModel();
            model.Advertisments = new List<UserAvailableAdvertismentResponseModel>();
            if (watchedAds.Count > 0)
            {
                model.UserId = watchedAds.FirstOrDefault().UserId;
                model.Name = watchedAds.FirstOrDefault().User.Name;
                foreach (var watchedAd in watchedAds)
                {
                    UserAvailableAdvertismentResponseModel adModel = _mapper.Map<UserAvailableAdvertismentResponseModel>(watchedAd.Advertisment);
                    adModel.LastWatchedAt = watchedAd.WatchDate;
                    adModel.IsAvailable = adModel.LastWatchedAt != null ? (DateTime.UtcNow >= adModel.LastWatchedAt.Value.AddHours(_adRenewHour)) : true;
                    model.Advertisments.Add(adModel);
                }
            }
            else
            {
                var user = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (user == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "User not found";

                    return response;
                }
                model.UserId = user.Id;
                model.Name = user.Name;
            }
            response.Status = ResponseStatusCodes.Success;
            response.Data = model;

            return response;
        }

        public async Task<ResponseModel> GetUserList(UserPaginationRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            PaginatedList<UserListResponseModel> paginatedUserList = new PaginatedList<UserListResponseModel>();
            try
            {
                var users = _context.Users.AsQueryable().Where(x => x.Role == Roles.User && (string.IsNullOrEmpty(request.Search) || x.Name.Contains(request.Search) || x.Phone.Contains(request.Search)));
                var totalCount = users.Count();
                var data = await users.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage == -1 ? totalCount : request.PerPage).Include(x => x.Memberships).Include(x => x.UserAdvertismentViews).ToListAsync();
                List<UserListResponseModel> userList = new List<UserListResponseModel>();
                foreach (var user in data)
                {
                    UserListResponseModel userResponse = new UserListResponseModel();
                    userResponse.Id = user.Id;
                    userResponse.Name = user.Name;
                    userResponse.Username = user.Phone;
                    userResponse.RenewalDate = user.Memberships.OrderByDescending(x => x.RenewalDate).Select(x => x.RenewalDate).FirstOrDefault();
                    userResponse.TotalWatchedAdvertisments = user.UserAdvertismentViews.Count();
                    userList.Add(userResponse);
                }

                paginatedUserList.Page = request.Page;
                paginatedUserList.PerPage = request.PerPage;
                paginatedUserList.Total = totalCount;
                paginatedUserList.TotalPages = totalCount / request.PerPage;
                paginatedUserList.Data = userList;

                response.Status = ResponseStatusCodes.Success;
                response.Data = paginatedUserList;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Data = ex.Message;
            }

            return response;

        }
    }
}
