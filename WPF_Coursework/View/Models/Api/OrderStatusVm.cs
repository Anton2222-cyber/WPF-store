using System;

namespace View.Models.Api {
	public class OrderStatusVm {
		public long Id { get; set; }
		public string Status { get; set; }
		public DateTime TimeOfCreation { get; set; }
	}
}
