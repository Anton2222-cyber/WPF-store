using Bogus;
using Data.Context;
using Data.Entities;
using WebApi.Services.Interfaces;

namespace WebApi.Services;

public class ContentSeeder(
	DataContext context,
	IConfiguration configuration
	) : IContentSeeder {

	private readonly Random random = new();

	public async Task<IEnumerable<Category>> SeedCategoriesAsync(int quantity) {
		var tasks = Enumerable.Range(0, quantity)
			.Select(i => GenerateCategoryAsync())
			.ToArray();

		var categories = await Task.WhenAll(tasks);

		await context.Categories.AddRangeAsync(categories);
		await context.SaveChangesAsync();

		return categories;
	}

	public async Task<IEnumerable<Product>> SeedProductsAsync(long categoryId, int quantity) {
		var tasks = Enumerable.Range(0, quantity)
			.Select(i => GenerateProductAsync(categoryId))
			.ToArray();

		var products = await Task.WhenAll(tasks);

		await context.Products.AddRangeAsync(products);
		try {
			await context.SaveChangesAsync();
		}
		catch {
			foreach (var p in products) {
				var createdImages = p.Images.Select(i => i.Name).ToArray();
				ImageWorker.DeleteImagesIfExists(createdImages);
			}

			throw;
		}

		return products;
	}

	private async Task<Category> GenerateCategoryAsync() {
		byte[] imageBytes = await GetImageBytesFromApiAsync();
		string imagePath = await ImageWorker.SaveImageAsync(imageBytes);

		var categoryGenerator = new Faker<Category>()
			.RuleFor(c => c.DateCreated, s => s.Date.Between(new DateTime(2020, 1, 1), DateTime.Now))
			.RuleFor(c => c.Name, s => $"Test category {Guid.NewGuid()}")
			.RuleFor(c => c.Description, s => s.Lorem.Lines(lineCount: 3))
			.RuleFor(c => c.Image, imagePath);

		return categoryGenerator.Generate();
	}

	private async Task<Product> GenerateProductAsync(long categoryId) {
		IEnumerable<Image> images = await GenerateImagesAsync(random.Next(1, 3));

		var productGenerator = new Faker<Product>()
			.RuleFor(p => p.CategoryId, categoryId)
			.RuleFor(p => p.DateCreated, s => s.Date.Between(new DateTime(2020, 1, 1), DateTime.Now))
			.RuleFor(p => p.Name, s => $"Test product {Guid.NewGuid()}")
			.RuleFor(p => p.Description, s => s.Lorem.Lines(lineCount: 3))
			.RuleFor(p => p.Price, s => random.Next(1, 10000))
			.RuleFor(p => p.Images, images);

		return productGenerator.Generate();
	}

	private async Task<IEnumerable<Image>> GenerateImagesAsync(int quantity) {
		var tasks = Enumerable.Range(1, quantity + 1)
			.Select(GenerateImageAsync)
			.ToArray();

		return await Task.WhenAll(tasks);
	}

	private async Task<Image> GenerateImageAsync(int order) {
		int maxAttemptsCount = 3;
		byte[]? imageBytes = null;

		for (int i = 0; i < maxAttemptsCount && imageBytes is null; i++) {
			try {
				imageBytes = await GetImageBytesFromApiAsync();
			}
			catch { }
		}

		if (imageBytes is null) {
			throw new Exception($"Image failed to load after {maxAttemptsCount} attempts");
		}

		string imagePath = await ImageWorker.SaveImageAsync(imageBytes);

		return new Image {
			Name = imagePath,
			Order = order
		};
	}

	private async Task<byte[]> GetImageBytesFromApiAsync() {
		using HttpClient client = new();

		string apiUrl = configuration["TestImagesApi:Url"]
			?? throw new NullReferenceException("TestImagesApi:Url is not contains in the appsettings.json");
		string size = configuration["TestImagesApi:ImageSize"]
			?? throw new NullReferenceException("TestImagesApi:ImageSize is not contains in the appsettings.json");

		return await client.GetByteArrayAsync($"{apiUrl}/{size}");
	}
}
