namespace WebApi.Models.Category;

public class CategoryUpdateViewModel {
	public int Id { get; set; }
	public string? Name { get; set; }
	public IFormFile? Image { get; set; }
	public string? Description { get; set; }
}