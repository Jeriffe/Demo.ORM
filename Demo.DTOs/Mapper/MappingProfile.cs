using AutoMapper;
using Demo.Data.Models;

namespace Demo.DTOs.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TPatient, Patient>()
                .ForMember(o => o.FullName, opt => opt.MapFrom(src => src.FirstName + "," + src.LastName))
                .ForMember(o => o.MedRecordNumber, opt => opt.MapFrom(src => src.MedRecNumber))
                .ReverseMap();

            CreateMap<TCustomer, Customer>()
                .ReverseMap(); 

            CreateMap<TOrder, Order>().ReverseMap(); 
            CreateMap<TOrderItem, OrderItem>()
                .ForMember(o => o.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ReverseMap(); 

            CreateMap<TProduct, Product>().ReverseMap(); 
        }
    }
}
