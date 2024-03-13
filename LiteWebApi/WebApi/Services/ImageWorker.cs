using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace WebApi.Services;

public static class ImageWorker {
	public static async Task<string> SaveImageAsync(IFormFile image) {
		using MemoryStream ms = new();
		await image.CopyToAsync(ms);
		string fileName = await SaveImageAsync(ms.ToArray());
		return fileName;
	}

	public static async Task<List<string>> SaveImagesAsync(ICollection<IFormFile> images) {
		List<string> result = [];

		try {
			foreach (var image in images) {
				result.Add(await SaveImageAsync(image));
			}
		}
		catch (Exception) {
			result.ForEach(DeleteImageIfExists);
			throw;
		}

		return result;
	}

	public static async Task<string> SaveImageAsync(string base64) {
		if (base64.Contains(','))
			base64 = base64.Split(',')[1];
		var bytes = Convert.FromBase64String(base64);
		var fileName = await SaveImageAsync(bytes);
		return fileName;
	}

	public static async Task<string> SaveImageAsync(byte[] bytes) {
		string imageName = Path.GetRandomFileName() + ".webp";
		string dirSaveImage = Path.Combine(ImagesDir, imageName);

		using (var image = Image.Load(bytes)) {
			image.Mutate(imageProcessingContext => {
				imageProcessingContext.Resize(new ResizeOptions {
					Size = new Size(Math.Min(image.Width, 1200), Math.Min(image.Height, 1200)),
					Mode = ResizeMode.Max
				});
			});

			using var stream = File.Create(dirSaveImage);
			await image.SaveAsync(stream, new WebpEncoder());
		}

		return imageName;
	}

	public static async Task<List<string>> SaveImagesAsync(ICollection<byte[]> bytesArray) {
		List<string> result = [];

		try {
			foreach (var bytes in bytesArray) {
				result.Add(await SaveImageAsync(bytes));
			}
		}
		catch (Exception) {
			result.ForEach(DeleteImageIfExists);
			throw;
		}

		return result;
	}

	public static bool IsImageExists(string name) {
		return File.Exists(Path.Combine(ImagesDir, name));
	}

	public static async Task<byte[]> LoadBytesAsync(string name) {
		return await File.ReadAllBytesAsync(Path.Combine(ImagesDir, name));
	}

	public static string ImagesDir => Path.Combine(Directory.GetCurrentDirectory(), "Data", "images");

	public static void DeleteImage(string nameWithFormat) {
		File.Delete(Path.Combine(ImagesDir, nameWithFormat));
	}

	public static void DeleteImages(ICollection<string> images) {
		foreach (var image in images)
			DeleteImage(image);
	}

	public static void DeleteImageIfExists(string nameWithFormat) {
		if (File.Exists(Path.Combine(ImagesDir, nameWithFormat))) {
			DeleteImage(nameWithFormat);
		}
	}

	public static void DeleteImagesIfExists(ICollection<string> images) {
		foreach (var image in images)
			DeleteImageIfExists(image);
	}
}
