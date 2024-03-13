namespace WebApi.Models.Category;

public class CategoryItemViewModel {
	public int Id { get; set; }
	public string Name { get; set; } = null!;
	public string Image { get; set; } = null!;
	public string Description { get; set; } = null!;
}
