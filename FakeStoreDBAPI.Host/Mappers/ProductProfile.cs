using AutoMapper;
using FakeStoreDBAPI.Host.DTO.Product;
using FakeStoreDBAPI.Host.Models;

namespace FakeStoreDBAPI.Host.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<CreateProductDto, Product>();
            CreateMap<Product, ProductDto>();

            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));
                
        }
    }
}
