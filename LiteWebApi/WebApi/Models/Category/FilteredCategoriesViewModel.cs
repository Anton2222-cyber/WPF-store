namespace WebApi.Models.Category;

public class FilteredCategoriesViewModel {
	public ICollection<CategoryItemViewModel> FilteredCategories { get; set; } = null!;
	public long AvailableCategories { get; set; }
}
