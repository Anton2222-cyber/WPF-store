using System.Collections.Generic;

namespace View.Models.Api {
	public class FilteredCategoriesVm {
		public ICollection<CategoryVm> FilteredCategories { get; set; }
		public long AvailableCategories { get; set; }
	}
}
