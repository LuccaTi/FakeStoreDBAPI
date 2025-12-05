using AutoMapper;
using FakeStoreDBAPI.Host.DTO.Order;
using FakeStoreDBAPI.Host.Models;

namespace FakeStoreDBAPI.Host.Mappers
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderDto>();
            CreateMap<Order, OrderWithCustomerDto>();

            CreateMap<Order, OrderWithOrderItemsDto>()
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderProducts));

            CreateMap<UpdateOrderDto, Order>()
                .ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<CreateOrderItemDto, OrderProduct>();
            CreateMap<OrderProduct, OrderItemDto>();
        }
    }
}
