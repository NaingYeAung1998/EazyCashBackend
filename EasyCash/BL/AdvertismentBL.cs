using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace EasyCash.BL
{
    public class AdvertismentBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;

        private readonly int _adRenewHour;

        public AdvertismentBL(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _adRenewHour = int.Parse(_config["AdRenewHour"]);
        }

        public async Task<ResponseModel> GetAllAdvertisments(AdvertismentPaginationRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            PaginatedList<AdvertismentResponseModel> adResponse = new PaginatedList<AdvertismentResponseModel>();
            try
            {
                var advertisments = _context.Advertisments.AsQueryable().Where(x => (string.IsNullOrEmpty(request.Search) || x.Title.Contains(request.Search)) && (x.AdvertismentStatus == request.Status || request.Status == null)).OrderByDescending(x=>x.CreatedAt);
                var totalCount = advertisments.Count();
                var data = await advertisments.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage == -1 ? totalCount : request.PerPage).ToListAsync();

                var adResponseData = _mapper.Map<List<AdvertismentResponseModel>>(data);
                adResponse.Page = request.Page;
                adResponse.PerPage = request.PerPage;
                adResponse.Total = totalCount;
                adResponse.TotalPages = totalCount / request.PerPage;
                adResponse.Data = adResponseData;

                response.Status = ResponseStatusCodes.Success;
                response.Data = adResponse;

            }catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            

            return response;
        }

        public async Task<ResponseModel> CreateAdvertisment(AdvertismentCreateRequestModel request, Guid userId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                Advertisment advertisment = _mapper.Map<AdvertismentCreateRequestModel, Advertisment>(request);
                advertisment.Id = Guid.NewGuid();
                advertisment.CreatedAt = DateTime.Now;
                advertisment.UpdatedAt = DateTime.Now;
                advertisment.CreatedBy = userId;

                _context.Advertisments.Add(advertisment);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = advertisment;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdateAdvertisment(AdvertismentUpdateRequestModel request, Guid userId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var advertisment = _context.Advertisments.Find(request.Id);
                if (advertisment == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Advertisment not Found";
                    return response;
                }
                advertisment.Title = request.Title;
                advertisment.Description = request.Description;
                advertisment.Link = request.Link;
                advertisment.EmbedLink = request.EmbedLink;
                advertisment.AmountPerWatch = request.AmountPerWatch;
                advertisment.AdvertismentStatus = request.AdvertismentStatus;
                advertisment.AdvertismentType = request.AdvertismentType;
                advertisment.UpdatedAt = DateTime.UtcNow;
                advertisment.UpdatedBy = userId;
                _context.Advertisments.Update(advertisment);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = advertisment;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> DeleteAdvertisment(Guid Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var advertisment = _context.Advertisments.Find(Id);
                if (advertisment == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Advertisment not Found";
                    return response;
                }

                _context.Advertisments.Remove(advertisment);
                await _context.SaveChangesAsync();
                response.Status = ResponseStatusCodes.Success;
                response.Data = advertisment;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> ChangeAdvertismentStatus(AdvertismentChangeStatusRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var advertisment = _context.Advertisments.Find(request.Id);
                if (advertisment == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Advertisment not Found";
                    return response;
                }
                advertisment.AdvertismentStatus = request.AdvertismentStatus;

                _context.Advertisments.Update(advertisment);
                await _context.SaveChangesAsync();
                response.Status = ResponseStatusCodes.Success;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> GetUserAvailableAdvertisments(Guid userId, AdvertismentPaginationRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            PaginatedList<UserAvailableAdvertismentResponseModel> adResponse = new PaginatedList<UserAvailableAdvertismentResponseModel>();
            try
            {
                var advertisments = await _context.Advertisments.AsQueryable()
                    .Where(x => (x.Title.Contains(request.Search) || string.IsNullOrEmpty(request.Search)) && (x.AdvertismentStatus == AdvertismentStatuses.Active)).OrderByDescending(x=>x.CreatedAt).ToListAsync();
                
                
                var watchedAdvertisments = await _context.UserAdvertismentViews.Where(x=>x.UserId == userId)
                    .GroupBy(y=> y.AdvertismentId).Select(z=>z.OrderByDescending(a=>a.WatchDate).First()).ToListAsync();

                List<UserAvailableAdvertismentResponseModel> adList = new List<UserAvailableAdvertismentResponseModel>();
                foreach(var advertisment in advertisments)
                {
                    UserAvailableAdvertismentResponseModel ad = new UserAvailableAdvertismentResponseModel();
                    var watchedAd = watchedAdvertisments.Where(x => x.AdvertismentId == advertisment.Id).FirstOrDefault();
                    ad = _mapper.Map<UserAvailableAdvertismentResponseModel>(advertisment);
                    ad.LastWatchedAt = watchedAd != null ? watchedAd.WatchDate : null;
                    ad.IsAvailable = ad.LastWatchedAt != null ? (DateTime.UtcNow >= ad.LastWatchedAt.Value.AddHours(_adRenewHour)) : true;
                    adList.Add(ad);
                }

                var totalCount = advertisments.Count();
                var data = adList.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage == -1 ? totalCount : request.PerPage).ToList();

                adResponse.Page = request.Page;
                adResponse.PerPage = request.PerPage;
                adResponse.Total = totalCount;
                adResponse.TotalPages = totalCount / request.PerPage;
                adResponse.Data = data;

                response.Status = ResponseStatusCodes.Success;
                response.Data = adResponse;

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ResponseModel> WatchAdvertisment(Guid userId, UserAdvertismentViewCreateRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var isAdAvailable = !_context.UserAdvertismentViews.Where(x => x.UserId == userId && x.AdvertismentId == request.AdvertismentId && DateTime.UtcNow < x.WatchDate.AddHours(_adRenewHour)).Any();
                if (!isAdAvailable)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "You have watched this ad within 24 hour.";

                    return response;
                }
                UserAdvertismentView userAdView = _mapper.Map<UserAdvertismentView>(request);
                userAdView.Id = Guid.NewGuid();
                userAdView.UserId = userId;
                userAdView.WatchDate = DateTime.UtcNow;
                _context.UserAdvertismentViews.Add(userAdView);

                var wallet = _context.Wallets.Where(x=>x.UserId == userId).FirstOrDefault();
                if (wallet == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Wallet Not Found";

                    return response;
                }
                wallet.Balance += userAdView.EarnedAmount;
                _context.Wallets.Update(wallet);

                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = userAdView;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
