using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class ImageUtil {
	public async static Task<Image<Rgba32>> getModImage(string url, ImageArgs? args) {
		args = args ?? new ImageArgs();

		// Retrieve image from stream
		Stream s = await new HttpClient().GetStreamAsync(url);
		Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(s);

		// Perform generic image operations
		Size size = new Size((int)(img.Width * args.Scale), (int)(img.Height * args.Scale));
		img.Mutate(accessor => accessor.Resize(size));

		return img;
	}
}