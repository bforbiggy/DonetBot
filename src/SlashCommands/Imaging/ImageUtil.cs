using Discord;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

public class ImageUtil
{
	// Retrieve image from https stream
	public async static Task<Image<Rgba32>> streamImage(string url)
	{
		Stream s = await new HttpClient().GetStreamAsync(url);
		return SixLabors.ImageSharp.Image.Load<Rgba32>(s);
	}

	// Resize image
	public static void resize(ref Image<Rgba32> img, double scale = 1.0)
	{
		if (scale == 1.0)
			return;
		Size size = new Size((int)(img.Width * scale), (int)(img.Height * scale));
		img.Mutate(accessor => accessor.Resize(size));
	}

	// Convert id to image
	public async static Task<Image<Rgba32>?> getImage(IUser? user)
	{
		if (user == null)
			return null;
		return await streamImage(user.GetAvatarUrl(size: 192));
	}

	// Convert url to image
	public async static Task<Image<Rgba32>?> getImage(string? url = null)
	{
		if (url == null)
			return null;
		return await streamImage(url);
	}

	// Convert url to image
	public async static Task<Image<Rgba32>?> getImage(IAttachment? attachment = null)
	{
		//TODO: Add check for content type
		if (attachment == null)
			return null;
		Console.WriteLine(attachment.ContentType);
		return await streamImage(attachment.Url);
	}
}