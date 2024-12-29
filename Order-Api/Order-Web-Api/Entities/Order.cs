using Order_Web_Api.Enums;

namespace Order_Web_Api.Entities
{
	public class Order
	{
        public Guid OrderId { get; set; }
        public Guid BuyerId { get; set; }
        public Decimal TotalPrice { get; set; }
        public OrderStatus OrderStatu { get; set; }
        public DateTime CreatedDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
