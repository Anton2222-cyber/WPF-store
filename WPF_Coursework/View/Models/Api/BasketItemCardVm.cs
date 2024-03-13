namespace View.Models.Api {
	public class BasketItemCardVm {
		public long Id { get; set; }

		public BasketProductCardVm Product { get; set; }

		public int Quantity { get; set; }
	}
}
