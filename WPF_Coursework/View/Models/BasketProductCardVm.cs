using System;
using View.Models.Api;

namespace View.Models {
	public class BasketProductCardVm {
		public long Id { get; set; }
		public DateTime DateCreated { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string TotalPrice { get; set; }
		public CategoryVm Category { get; set; }
		public string Image { get; set; }
	}
}
