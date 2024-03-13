using SixLabors.ImageSharp;

namespace WebApi.Services;

public static class ImageValidator {
	public static bool IsValidImage(IFormFile image) {
		using var stream = image.OpenReadStream();

		try {
			using var imageInstance = Image.Load(stream);
		}
		catch {
			return false;
		}
		return true;
	}

	public static bool IsValidImages(ICollection<IFormFile> images) => images.All(IsValidImage);
}
