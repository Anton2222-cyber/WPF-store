using System;
using View.Models.Api;

namespace View.Models {
	public class ProductCardVm {
		public long Id { get; set; }
		public DateTime DateCreated { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public CategoryVm Category { get; set; }
		public string Image { get; set; }
	}
}
