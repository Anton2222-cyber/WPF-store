namespace View.Models.Api {
	public class BasketItemVm {
		public long Id { get; set; }

		public ProductItemVm Product { get; set; }

		public int Quantity { get; set; }
	}
}
