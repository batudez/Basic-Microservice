using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order_Web_Api.Context;
using Order_Web_Api.Entities;
using Order_Web_Api.ViewModels;
using Shared.Events;

namespace Order_Web_Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		readonly OrderApiDbContext _context;
		readonly IPublishEndpoint _publishEndpoint;

		public OrderController(OrderApiDbContext context, IPublishEndpoint publishEndpoint)
		{
			_context = context;
			_publishEndpoint = publishEndpoint;
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(CreateOrderVM createOrder)
		{
			Order order = new()
			{
				OrderId = Guid.NewGuid(),
				BuyerId = createOrder.BuyerId,
				CreatedDate = DateTime.UtcNow,
				OrderStatu = Enums.OrderStatus.Suspend,
			};

			order.OrderItems = createOrder.OrderItems.Select(oi => new OrderItem
			{
				Count = oi.Count,
				Price = oi.Price,
				ProductId = oi.ProductId,
			}).ToList();
			
			order.TotalPrice = createOrder.OrderItems.Sum(oi => oi.Price * oi.Count);
			await _context.Orders.AddAsync(order);
			await _context.SaveChangesAsync();

			OrderCreatedEvent orderCreatedEvent = new()
			{
				BuyerId = order.BuyerId,
				OrderId = order.OrderId,
				OrderItems = order.OrderItems.Select(oi => new Shared.Messages.OrderItemMessage
				{
					Count = oi.Count,
					ProductId= oi.ProductId,
				}).ToList(),
				TotalPrice = order.TotalPrice,
			};
			await _publishEndpoint.Publish(orderCreatedEvent);
			return Ok();
		}
	}
}
