using System;
using System.Collections.Generic;

namespace View.Models.Api {
	public class ProductItemVm {
		public long Id { get; set; }
		public DateTime DateCreated { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public double Price { get; set; }
		public CategoryVm Category { get; set; }
		public ICollection<string> Images { get; set; }
	}
}
