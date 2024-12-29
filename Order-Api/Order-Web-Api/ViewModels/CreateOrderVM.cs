namespace Order_Web_Api.ViewModels
{
	public class CreateOrderVM
	{
		public Guid BuyerId { get; set; }
        public ICollection<CreateOrderItemVM> OrderItems { get; set; }

    }
}
