using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order_Web_Api.Context;
using Order_Web_Api.Entities;
using Shared.Events;

namespace Order_Web_Api.Consumers
{
	public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
	{
		readonly OrderApiDbContext _orderApiDbContext;

		public PaymentFailedEventConsumer(OrderApiDbContext orderApiDbContext)
		{
			_orderApiDbContext = orderApiDbContext;
		}

		public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
		{
			Order order = await _orderApiDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
			order.OrderStatu = Enums.OrderStatus.Failed;
			await _orderApiDbContext.SaveChangesAsync();
		}
	}
}
