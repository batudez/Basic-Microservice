using MassTransit;
using Shared.Events;

namespace Payment_Web_Api.Consumers
{
	public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
	{
		readonly IPublishEndpoint _publishEndpoint;

		public StockReservedEventConsumer(IPublishEndpoint publishEndpoint)
		{
			_publishEndpoint = publishEndpoint;
		}

		public Task Consume(ConsumeContext<StockReservedEvent> context)
		{
			if (true)
			{
				PaymentCompletedEvent paymentCompletedEvent = new()
				{
					OrderId = context.Message.OrderId
				};
				_publishEndpoint.Publish(paymentCompletedEvent);

                Console.WriteLine("odeme basarili");
            }
			else
			{
				PaymentFailedEvent paymentFailedEvent = new()
				{
					OrderId = context.Message.OrderId,
					Message = "payment failed",
				};
                Console.WriteLine("odeme basarisiz");
            }
			return Task.CompletedTask;
		}
	}
}
