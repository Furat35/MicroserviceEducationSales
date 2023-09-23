using AutoMapper;
using Services.Order.Application.Dtos;
using Services.Order.Domain.OrderAggregate;

namespace Services.Order.Application.Mappings
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Domain.OrderAggregate.Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}
