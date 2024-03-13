using System.Collections.Generic;

namespace View.Models.Api {
	public class FilteredOrdersVm {
		public ICollection<OrderVm> FilteredOrders { get; set; }
		public long AvailableQuantity { get; set; }
	}
}
