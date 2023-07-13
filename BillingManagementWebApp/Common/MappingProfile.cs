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
            CreateMap<User,UserViewModel>().ReverseMap();
            CreateMap<User, CreateUserViewModel>().ReverseMap();

            CreateMap<Flat, FlatViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                                            .ForMember(dest=>dest.UserTc,opt=>opt.MapFrom(src=>src.User.TCNo));
            CreateMap<FlatViewModel, Flat>().ForMember(dest => dest.UserId, opt => opt.Ignore())
                                            .ForMember(dest=>dest.User,opt=>opt.Ignore());

            CreateMap<Due, DueViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                                          .ForMember(dest => dest.UserTc, opt => opt.MapFrom(src => src.User.TCNo));
            CreateMap<DueViewModel, Due>().ForMember(dest => dest.UserId, opt => opt.Ignore())
                                          .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Invoice, InvoiceViewModel>().ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User.Name + " " + src.User.Surname))
                                                  .ForMember(dest => dest.UserTc, opt => opt.MapFrom(src => src.User.TCNo));
            CreateMap<InvoiceViewModel, Invoice>().ForMember(dest => dest.UserId, opt => opt.Ignore())
                                                  .ForMember(dest => dest.User, opt => opt.Ignore());


            CreateMap<Message, MessageViewModel>().ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender.Name + " " + src.Sender.Surname))
                                                  .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver.Name + " " + src.Receiver.Surname));
            CreateMap<MessageViewModel, Message>().ForMember(dest => dest.SenderId, opt => opt.Ignore())
                                                  .ForMember(dest => dest.Sender, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ReceiverId, opt => opt.Ignore())
                                                  .ForMember(dest => dest.Receiver, opt => opt.Ignore());
        }
    }

}