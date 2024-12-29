namespace Order_Web_Api.ViewModels
{
	public class CreateOrderItemVM
	{
		public string ProductId { get; set; }
		public int Count { get; set; }
		public Decimal Price { get; set; }
	}
}
