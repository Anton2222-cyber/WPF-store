namespace View.Models.Api {
	public class OrderedProductVm {
		public long Id { get; set; }
		public ProductVm Product { get; set; }
		public double UnitPrice { get; set; }
		public int Quantity { get; set; }
	}
}
