using EasyCash.Entities;
using EasyCash.Models;
using EasyCash.Models.Request;
using EasyCash.Models.Response;
using EasyCash.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EasyCash.BL
{
    public class AuthenticationBL
    {
        //Database
        private readonly ApplicationDbContext _context;
        //Configuration
        private readonly IConfiguration _config;

        private readonly EncryptionServices _encryptionServices;
        //Constructor
        public AuthenticationBL(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _encryptionServices = new EncryptionServices(_config);
        }

        //Admin and Customer Login
        public ResponseModel Login(LoginRequestModel loginInfo)
        {
            ResponseModel response = new ResponseModel();
            //Find user by username and password
            var encryptedPassword = _encryptionServices.Encrypt(loginInfo.Password);
            var user = _context.Users.Where(x => x.Phone == loginInfo.Username && x.Password == encryptedPassword).FirstOrDefault();

            LoginResponseModel model = new LoginResponseModel();

            if (user == null)
            {
                //Return incorrect username or password if user not found
                response.Status = ResponseStatusCodes.Fail;
                model.StatusCode = AuthStatusCodes.IncorrectUsernameOrPassword;
                model.Token = "";
            }

            else
            {
                //Generate token and return to user
                response.Status = ResponseStatusCodes.Success;
                model.StatusCode = AuthStatusCodes.Success;
                if(user.Role == Roles.User)
                {
                    var walletId = _context.Wallets.Where(x => x.UserId == user.Id).Select(x=>x.Id).FirstOrDefault();
                    model.Token = GenerateJwtToken(user.Phone, user.Id, user.Role.ToString(), walletId);
                }
                else
                {
                    model.Token = GenerateJwtToken(user.Phone, user.Id, user.Role.ToString());
                }
                
                model.Role = user.Role.ToString();
            }
            response.Data = model;

            return response;
        }

        //Admin and Customer register
        public ResponseModel Register(RegisterWithMembershipRequestModel registerInfo)
        {
            ResponseModel response = new ResponseModel();

            //Check if user is already existed
            var existedUser = _context.Users.Where(x => x.Phone == registerInfo.Username).FirstOrDefault();
            RegisterResponseModel model = new RegisterResponseModel();

            if (existedUser != null)
            {
                //Return unavailable username if user already existed
                response.Status = ResponseStatusCodes.Fail;
                model.StatusCode = AuthStatusCodes.UnavailableUsername;
                model.Token = "";
            }

            else if (registerInfo.Password != registerInfo.ConfirmPassword)
            {
                //Return password not match if password and confirm password not the same
                response.Status = ResponseStatusCodes.Fail;
                model.StatusCode = AuthStatusCodes.PasswordsNotMatch;
                model.Token = "";
            }
            else
            {
                //Create new user
                User newUser = new User();
                newUser.Id = Guid.NewGuid();
                newUser.Phone = registerInfo.Username;
                newUser.Name = registerInfo.Name;
                newUser.Password = _encryptionServices.Encrypt(registerInfo.Password);
                newUser.Role = Roles.Admin;
                _context.Users.Add(newUser);

                if(newUser.Role == Roles.User)
                {
                    //If Role is User, create Wallet
                    Wallet wallet = new Wallet();
                    wallet.Id = Guid.NewGuid();
                    wallet.WalletNumber = Create16DigitString();
                    wallet.UserId = newUser.Id;
                    wallet.Balance = 0;
                    _context.Wallets.Add(wallet);

                    //If Role is User, create Membership
                    Membership membership = new Membership();
                    membership.Id = Guid.NewGuid();
                    membership.UserId = newUser.Id;
                    membership.SubscriptionDate = registerInfo.SubscriptionDate??DateTime.UtcNow;
                    membership.RenewalDate = membership.SubscriptionDate.AddMonths(registerInfo.TotalMonth??1);
                    membership.SubscriptionAmount = registerInfo.SubscriptionAmount ?? (registerInfo.TotalMonth ?? 1 * decimal.Parse(_config["MembershipFees"]));
                    _context.Memberships.Add(membership);

                    model.Token = GenerateJwtToken(newUser.Phone, newUser.Id, newUser.Role.ToString(), wallet.Id);
                }
                else
                {
                    model.Token = GenerateJwtToken(newUser.Phone, newUser.Id, newUser.Role.ToString());
                }
                

                _context.SaveChanges();

                response.Status = ResponseStatusCodes.Success;
                model.StatusCode = AuthStatusCodes.Success;
                
                model.Role = newUser.Role.ToString();
            }
            response.Data = model;

            return response;
        }

        private object GenerateJwtToken(string username, Guid userid, string role, Guid? walletId = null)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userid.ToString()),
                new Claim(ClaimTypes.Role, role)
            };
            if(walletId != null )
            {
                claims.Add(new Claim(ClaimTypes.SerialNumber, walletId.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_config["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _config["JwtIssuer"],
                _config["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string Create16DigitString()
        {
            Random RNG = new Random();
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }
            return builder.ToString();
        }
    }
}
