using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using EasyCash.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyCash.BL
{
    public class PaymentMethodBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;

        public PaymentMethodBL(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ResponseModel> GetAllPaymentMethods(PaymentMethodPaginationRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            PaginatedList<PaymentMethodResponseModel> paymentResponse = new PaginatedList<PaymentMethodResponseModel>();
            try
            {
                var paymentMethods = _context.PaymentMethods.AsQueryable().Where(x => x.Name.Contains(request.Search) || string.IsNullOrEmpty(request.Search));
                var totalCount = paymentMethods.Count();
                var data = await paymentMethods.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage == -1 ? totalCount : request.PerPage).ToListAsync();

                var paymentResponseData = _mapper.Map<List<PaymentMethodResponseModel>>(data);
                paymentResponse.Page = request.Page;
                paymentResponse.PerPage = request.PerPage;
                paymentResponse.Total = totalCount;
                paymentResponse.TotalPages = totalCount / request.PerPage;
                paymentResponse.Data = paymentResponseData;

                response.Status = ResponseStatusCodes.Success;
                response.Data = paymentResponse;

            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }


            return response;
        }

        public async Task<ResponseModel> CreatePaymentMethod(PaymentMethodCreateRequestModel request, Guid userId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                PaymentMethod payment = _mapper.Map<PaymentMethodCreateRequestModel, PaymentMethod>(request);
                payment.Id = Guid.NewGuid();
                payment.CreatedAt = DateTime.Now;
                payment.UpdatedAt = DateTime.Now;
                payment.CreatedBy = userId;

                _context.PaymentMethods.Add(payment);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = payment;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> UpdatePaymentMethod(PaymentMethodUpdateRequestModel request, Guid userId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var payment = _context.PaymentMethods.Find(request.Id);
                if (payment == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Payment Method not Found";
                    return response;
                }
                payment.Name = request.Name;
                payment.Description = request.Description;
                payment.UpdatedAt = DateTime.UtcNow;
                payment.UpdatedBy = userId;
                _context.PaymentMethods.Update(payment);
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

        public async Task<ResponseModel> DeletePaymentMethod(Guid Id)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var payment = _context.PaymentMethods.Find(Id);
                if (payment == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Payment Method not Found";
                    return response;
                }

                _context.PaymentMethods.Remove(payment);
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
    }
}
