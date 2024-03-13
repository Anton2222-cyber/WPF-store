using System.Collections.Generic;

namespace View.Models.Api {
	public class OrderCardVm {
		public long Id { get; set; }
		public string TimeOfCreation { get; set; }
		public OrderStatusCardVm Status { get; set; }
		public PostOfficeVm PostOffice { get; set; }
		public ICollection<OrderedProductCardVm> OrderedProducts { get; set; }
	}
}
