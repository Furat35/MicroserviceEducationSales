using FreeCourse.Services.Order.Domain.Core;
using Services.Order.Domain.Core;

namespace Services.Order.Domain.OrderAggregate
{
    public class Order : Entity, IAggregateRoot
    {
        public DateTime CreatedDate { get; private set; }
        public Address Address { get; private set; }
        public string BuyerId { get; private set; }
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        private readonly List<OrderItem> _orderItems;

        public Order()
        {

        }

        public Order(Address address, string buyerId)
        {
            CreatedDate = DateTime.Now;
            Address = address;
            BuyerId = buyerId;
            _orderItems = new();
        }

        public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
        {
            var existProduct = _orderItems.Any(_ => _.ProductId == productId);
            if (existProduct)
            {
                var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
                _orderItems.Add(newOrderItem);
            }
        }

        public decimal GetTotalPrice => _orderItems.Sum(_ => _.Price);
    }
}
