namespace View.Models.Api {
	public class ProductFilterVm {
		public long? CategoryId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double? MinPrice { get; set; }
		public double? MaxPrice { get; set; }

		public int? Offset { get; set; }
		public int? Limit { get; set; }
	}
}
