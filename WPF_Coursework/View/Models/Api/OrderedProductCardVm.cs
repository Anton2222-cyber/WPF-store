namespace View.Models.Api {
	public class OrderedProductCardVm {
		public long Id { get; set; }
		public ProductCardVm Product { get; set; }
		public string UnitPrice { get; set; }
		public string Quantity { get; set; }
	}
}
