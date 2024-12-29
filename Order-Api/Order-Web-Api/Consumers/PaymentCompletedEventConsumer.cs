using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order_Web_Api.Context;
using Order_Web_Api.Entities;
using Shared.Events;

namespace Order_Web_Api.Consumers
{
	public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
	{
		readonly OrderApiDbContext _orderApiDbContext;

		public PaymentCompletedEventConsumer(OrderApiDbContext orderApiDbContext)
		{
			_orderApiDbContext = orderApiDbContext;
		}

		public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
		{
			Order order = await _orderApiDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
			order.OrderStatu = Enums.OrderStatus.Completed;
			await _orderApiDbContext.SaveChangesAsync();
		}
	}
}
