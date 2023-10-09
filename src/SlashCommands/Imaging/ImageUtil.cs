using Discord;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

[DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
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

	// Converts image to file attachment
	public static FileAttachment ToAttachment(Image<Rgba32> img, string name = "unknown")
	{
		// Properly set file extension
		name = System.IO.Path.ChangeExtension(name, ".png");

		// Convert img to file attachment
		MemoryStream ms = new MemoryStream();
		img.Save(ms, new PngEncoder());
		return new FileAttachment(ms, name);
	}

	// Convert id to image
	public async static Task<Image<Rgba32>?> getImage(IUser? user)
	{
		if (user == null)
			return null;
		return await streamImage(user.GetAvatarUrl(size: 256));
	}

	// Convert url to image
	public async static Task<Image<Rgba32>?> getImage(string? url = null)
	{
		if (url == null)
			return null;
		return await streamImage(url);
	}

	// Convert attachment url to image
	public async static Task<Image<Rgba32>?> getImage(IAttachment? attachment = null)
	{
		// Ensure the attachment is actually an image
		if (attachment == null || attachment.ContentType == null || !attachment.ContentType.StartsWith("image"))
			return null;
		return await streamImage(attachment.Url);
	}
}