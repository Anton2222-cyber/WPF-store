using System;
using System.Collections.Generic;

namespace View.Models.Api {
	public class OrderVm {
		public long Id { get; set; }
		public DateTime TimeOfCreation { get; set; }
		public OrderStatusVm Status { get; set; }
		public PostOfficeVm PostOffice { get; set; }
		public ICollection<OrderedProductVm> OrderedProducts { get; set; }
	}
}
