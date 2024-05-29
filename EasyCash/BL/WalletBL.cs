using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace EasyCash.BL
{
    public class WalletBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;
        //Mapper
        private readonly IMapper _mapper;

        private readonly decimal _withdrawLimit;

        public WalletBL(ApplicationDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _withdrawLimit = decimal.Parse(_config["WithdrawLimit"]);
        }

        public async Task<ResponseModel> GetWalletBalance(Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                WalletBalanceResponseModel walletBalance = new WalletBalanceResponseModel();
                var wallet = await _context.Wallets.Where(x=>x.Id == walletId).FirstOrDefaultAsync();
                if(wallet == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Wallet Not Found";
                    return response;
                }
                walletBalance.Id = wallet.Id;
                walletBalance.WalletNumber = wallet.WalletNumber;
                walletBalance.Balance = wallet.Balance;

                response.Status = ResponseStatusCodes.Success;
                response.Data = walletBalance;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> GetWalletInfo(Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                WalletInfoResponseModel walletInfo = new WalletInfoResponseModel();
                var wallet = await _context.Wallets.Where(x => x.Id == walletId).Include(x=>x.WalletTransactions).Include(x=>x.WalletPaymentMethods).ThenInclude(z=>z.PaymentMethod).Include(x=>x.User).ThenInclude(y=>y.Memberships).FirstOrDefaultAsync();
                if (wallet == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Wallet Not Found";
                    return response;
                }
                walletInfo = _mapper.Map<WalletInfoResponseModel>(wallet);
                if(wallet.User == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "User Not Found";
                    return response;
                }
                walletInfo.Name = wallet.User!=null?wallet.User.Name:"";
                walletInfo.WalletTransactions = walletInfo.WalletTransactions.OrderByDescending(x => x.CreatedAt).ToList();
                if(wallet.User.Memberships != null && wallet.User.Memberships.Count() > 0)
                {
                    walletInfo.ExpiryDate = wallet.User.Memberships.OrderByDescending(x=>x.RenewalDate).FirstOrDefault().RenewalDate;
                }
                else
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Membership Not Found";
                    return response;
                }

                response.Status = ResponseStatusCodes.Success;
                response.Data = walletInfo;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> GetWalletPaymentMethods(Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var paymentMethods = await _context.Wallets.Where(x => x.Id == walletId).Include(x=>x.WalletPaymentMethods).ThenInclude(y=>y.PaymentMethod).Select(x => x.WalletPaymentMethods).FirstOrDefaultAsync();
                if(paymentMethods == null)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Wallet Not Found";

                    return response;
                }
                response.Status = ResponseStatusCodes.Success;
                response.Data = paymentMethods;

            }
            catch(Exception ex) 
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ResponseModel> AddWalletPaymentMethod(WalletPaymentMethodCreateRequestModel request, Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var isPaymentMethodExisted = _context.WalletPaymentMethods.Where(x => x.WalletId == walletId && x.PaymentMethodId == request.PaymentMethodId && x.AccountNumber == request.AccountNumber).Any();
                if (isPaymentMethodExisted)
                {
                    response.Status = ResponseStatusCodes.Fail;
                    response.Message = "Account Already Existed";

                    return response;
                }
                WalletPaymentMethod walletPaymentMethod = _mapper.Map<WalletPaymentMethod>(request);
                walletPaymentMethod.Id = Guid.NewGuid();
                walletPaymentMethod.WalletId = walletId;
                walletPaymentMethod.CreatedAt = DateTime.UtcNow;
                walletPaymentMethod.UpdatedAt = DateTime.UtcNow;
                _context.WalletPaymentMethods.Add(walletPaymentMethod);
                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = walletPaymentMethod;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ResponseModel> Withdraw(WithdrawRequestModel request, Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            var wallet = _context.Wallets.Where(x => x.Id == walletId).FirstOrDefault();
            if (wallet == null)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = "Wallet not found";
                return response;
            }
            if(wallet.Balance < _withdrawLimit)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = "Balance lower than withdraw limit";
                return response;
            }
            if(request.Amount > wallet.Balance)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = "Withdraw amount exceeds balance";
                return response;
            }
            try
            {
                Transaction tx = new Transaction();
                tx.Id = Guid.NewGuid();
                tx.WalletId = walletId;
                tx.WalletPaymentMethodId = request.PaymentMethodId;
                tx.Amount = request.Amount;
                tx.TransactionType = TransactionTypes.Withdraw;
                tx.TransactionStatus = TransactionStatuses.Requested;
                tx.CreatedAt = DateTime.UtcNow;
                tx.UpdatedAt = DateTime.UtcNow;
                _context.Transactions.Add(tx);

                wallet.Balance -= request.Amount;
                _context.Wallets.Update(wallet);

                await _context.SaveChangesAsync();

                response.Status = ResponseStatusCodes.Success;
                response.Data = tx;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }

            return response;

        }

        public async Task<ResponseModel> GetWalletTransactions(Guid walletId)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var txs = await _context.Transactions.Where(x => x.WalletId == walletId).OrderByDescending(x => x.CreatedAt).Take(5).Include(x => x.WalletPaymentMethod).ToListAsync();
                response.Status = ResponseStatusCodes.Success;
                response.Data = txs;
            }
            catch(Exception ex)
            {
                response.Status = ResponseStatusCodes.Fail;
                response.Message = ex.Message;
            }

            return response;
        }

        
    }
}
