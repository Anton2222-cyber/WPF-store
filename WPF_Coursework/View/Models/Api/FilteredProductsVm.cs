using System.Collections.Generic;

namespace View.Models.Api {
	public class FilteredProductsVm {
		public ICollection<ProductVm> FilteredProducts { get; set; }
		public long AvailableQuantity { get; set; }
	}

}
