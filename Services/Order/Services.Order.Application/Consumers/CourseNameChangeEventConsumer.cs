using MassTransit;
using Microsoft.EntityFrameworkCore;
using Services.Order.Infrastructure;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Order.Application.Consumers
{
    public class CourseNameChangeEventConsumer : IConsumer<CourseNameChangedEvent>
    {
        private readonly OrderDbContext _dbContext;

        public CourseNameChangeEventConsumer(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var orderItems = await _dbContext.OrderItems.Where(oi => oi.ProductId == context.Message.CourseId).ToListAsync();
            orderItems.ForEach(oi =>
            {
                oi.UpdateOrderItem(context.Message.UpdatedName, oi.PictureUrl, oi.Price);
            });
            await _dbContext.SaveChangesAsync();
        }
    }
}
