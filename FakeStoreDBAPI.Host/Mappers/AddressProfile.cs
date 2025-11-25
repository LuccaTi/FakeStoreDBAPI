using AutoMapper;
using FakeStoreDBAPI.Host.DTO.Address;
using FakeStoreDBAPI.Host.Models;

namespace FakeStoreDBAPI.Host.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<CreateAddressDto, Address>();
            CreateMap<Address, AddressDto>();

            CreateMap<UpdateAddressDto, Address>()
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
