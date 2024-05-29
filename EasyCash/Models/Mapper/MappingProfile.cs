using AutoMapper;
using EasyCash.Entities;
using EasyCash.Models.Request;
using EasyCash.Models.Response;

namespace EasyCash.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //Advertisment Mapping
            CreateMap<Advertisment, AdvertismentResponseModel>();
            CreateMap<AdvertismentCreateRequestModel, Advertisment>();
            CreateMap<Advertisment, UserAvailableAdvertismentResponseModel>();

            //PaymentMethod Mapping
            CreateMap<PaymentMethod, PaymentMethodResponseModel>();
            CreateMap<PaymentMethodCreateRequestModel, PaymentMethod>();

            //UserAdvertismentView Mapping
            CreateMap<UserAdvertismentViewCreateRequestModel, UserAdvertismentView>();

            //Wallet Mapping
            CreateMap<Wallet, WalletInfoResponseModel>();
            
            //WalletPaymentMethod Mapping
            CreateMap<WalletPaymentMethodCreateRequestModel, WalletPaymentMethod>();

        }
    }
}
