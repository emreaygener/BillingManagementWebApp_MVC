using AutoMapper;
using BillingManagementWebApp.Models;
using BillingManagementWebApp.Models.ViewModels;
using System.Data.SqlTypes;

namespace BillingManagementWebApp.Common
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserViewModel>();
            CreateMap<User, CreateUserViewModel>();

            CreateMap<Flat, FlatViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname));
            CreateMap<FlatViewModel, Flat>();

            CreateMap<Due, DueViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname));
            CreateMap<DueViewModel, Due>();

            CreateMap<Invoice, InvoiceViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname)); ;
            CreateMap<InvoiceViewModel, Invoice>();


            CreateMap<Message, MessageViewModel>().ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender.Name + " " + src.Sender.Surname))
                                                  .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver.Name + " " + src.Receiver.Surname));
            CreateMap<MessageViewModel, Message>();
        }
    }

}