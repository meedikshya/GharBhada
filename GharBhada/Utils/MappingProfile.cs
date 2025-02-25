using AutoMapper;
using GharBhada.Models;
using GharBhada.DTOs.AgreementDTOs;
using GharBhada.DTOs.BookingDTOs;
using GharBhada.DTOs.FavouriteDTOs;
using GharBhada.DTOs.MoveInAssistanceDTOs;
using GharBhada.DTOs.PaymentDTOs;
using GharBhada.DTOs.PropertyDTOs;
using GharBhada.DTOs.PropertyImageDTOs;
using GharBhada.DTOs.UserDTOs;
using GharBhada.DTOs.UserDetailDTOs;
using GharBhada.DTOs;

namespace GharBhada.Utils
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Agreement mappings
            CreateMap<Agreement, AgreementReadDTO>();
            CreateMap<Agreement, AgreementCreateDTO>();
            CreateMap<AgreementCreateDTO, Agreement>();
            CreateMap<Agreement, AgreementUpdateDTO>();
            CreateMap<AgreementUpdateDTO, Agreement>();

            // Booking mappings
            CreateMap<Booking, BookingReadDTO>();
            CreateMap<Booking, BookingCreateDTO>();
            CreateMap<BookingCreateDTO, Booking>();
            CreateMap<Booking, BookingUpdateDTO>();
            CreateMap<BookingUpdateDTO, Booking>();

            // Favourite mappings
            CreateMap<Favourite, FavouriteReadDTO>();
            CreateMap<Favourite, FavouriteCreateDTO>();
            CreateMap<FavouriteCreateDTO, Favourite>();
            CreateMap<Favourite, FavouriteUpdateDTO>();  
            CreateMap<FavouriteUpdateDTO, Favourite>();

            // Move-in Assistance mappings
            CreateMap<MoveInAssistance, MoveInAssistanceReadDTO>();
            CreateMap<MoveInAssistance, MoveInAssistanceCreateDTO>();
            CreateMap<MoveInAssistanceCreateDTO, MoveInAssistance>();
            CreateMap<MoveInAssistance, MoveInAssistanceUpdateDTO>();  
            CreateMap<MoveInAssistanceUpdateDTO, MoveInAssistance>();  


            // Payment mappings
            CreateMap<Payment, PaymentReadDTO>();
            CreateMap<Payment, PaymentCreateDTO>();
            CreateMap<PaymentCreateDTO, Payment>();
            CreateMap<Payment, PaymentUpdateDTO>();
            CreateMap<PaymentUpdateDTO, Payment>();

            // Property mappings
            CreateMap<Property, PropertyReadDTO>();
            CreateMap<Property, PropertyCreateDTO>();
            CreateMap<PropertyCreateDTO, Property>();
            CreateMap<Property, PropertyUpdateDTO>();
            CreateMap<PropertyUpdateDTO, Property>();

            // Property Image mappings
            CreateMap<PropertyImage, PropertyImageReadDTO>();
            CreateMap<PropertyImage, PropertyImageCreateDTO>();
            CreateMap<PropertyImageCreateDTO, PropertyImage>();
            CreateMap<PropertyImage, PropertyImageUpdateDTO>();  
            CreateMap<PropertyImageUpdateDTO, PropertyImage>(); 

            // User mappings
            CreateMap<User, UserReadDTO>();
            CreateMap<User, UserCreateDTO>();
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserUpdateDTO>();
            CreateMap<UserUpdateDTO, User>();

            // User Detail mappings
            CreateMap<UserDetail, UserDetailReadDTO>();
            CreateMap<UserDetail, UserDetailCreateDTO>();
            CreateMap<UserDetailCreateDTO, UserDetail>();
            CreateMap<UserDetail, UserDetailUpdateDTO>();
            CreateMap<UserDetailUpdateDTO, UserDetail>();
        }
    }
}
