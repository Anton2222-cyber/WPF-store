using Data.Entities;

namespace WebApi.Services.Interfaces;

public interface IContentSeeder {
	Task<IEnumerable<Category>> SeedCategoriesAsync(int quantity);
	Task<IEnumerable<Product>> SeedProductsAsync(long categoryId, int quantity);
}
