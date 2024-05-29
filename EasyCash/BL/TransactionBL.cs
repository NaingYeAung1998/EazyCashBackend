using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models.Request;
using EasyCash.Models;
using Microsoft.EntityFrameworkCore;
using EasyCash.Models.Response;

namespace EasyCash.BL
{
    public class TransactionBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;

        private readonly decimal _withdrawLimit;

        public TransactionBL(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _withdrawLimit = decimal.Parse(_config["WithdrawLimit"]);
        }

        public async Task<ResponseModel> GetTransactionList(TransactionPaginationRequestModel request)
        {
            ResponseModel response = new ResponseModel();
            PaginatedList<Transaction> paginatedList = new PaginatedList<Transaction>();
            try
            {
                var txs = _context.Transactions.AsQueryable().Include(x=>x.Wallet).Include(x => x.WalletPaymentMethod).ThenInclude(y => y.PaymentMethod).OrderByDescending(x=>x.CreatedAt);
                var totalCount = txs.Count();
                var data = await txs.Skip((request.Page - 1) * request.PerPage).Take(request.PerPage == -1 ? totalCount : request.PerPage).ToListAsync();

                paginatedList.Page = request.Page;
                paginatedList.PerPage = request.PerPage;
                paginatedList.Total = totalCount;
                paginatedList.TotalPages = totalCount / request.PerPage;
                paginatedList.Data = data;
                response.Status = ResponseStatusCodes.Success;
                response.Data = paginatedList;
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
