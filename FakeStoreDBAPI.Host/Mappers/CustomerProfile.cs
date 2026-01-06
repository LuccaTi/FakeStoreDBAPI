using AutoMapper;
using FakeStoreDBAPI.Host.DTO.Customer;
using FakeStoreDBAPI.Host.Models;

namespace FakeStoreDBAPI.Host.Mappers
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<Customer, CustomerWithAddressDto>();

            CreateMap<UpdateCustomerDto, Customer>()
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Customer, CustomerWithOrdersDto>()
                .ForMember(dest => dest.CustomerOrders, opt => opt.MapFrom(src => src.CustomerOrders));
        }
    }
}
