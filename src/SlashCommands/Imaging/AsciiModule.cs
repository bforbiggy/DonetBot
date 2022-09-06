using Discord;
using Discord.Interactions;
using image_filter_tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text;

public class AsciiModule : InteractionModuleBase
{
	[SlashCommand("ascii", "Converts an image to ascii.")]
	public async Task AsciiAsync(
		[Summary("user", "Choose user's profile picture")] IUser? user = null,
		[Summary("url", "Link to image")] string? url = null,
		[Summary("upload", "Upload custom image")] IAttachment? attachment = null,
		[Summary("scale", "Image size multiplier")] double scale = 1.0)
	{
		Image<Rgba32>?[] images = { await ImageUtil.getImage(user), await ImageUtil.getImage(url), await ImageUtil.getImage(attachment) };
		LinkedList<FileAttachment> attachments = new LinkedList<FileAttachment>();

		// Convert all images to ascii
		await RespondAsync("Processing images...");
		for (int i = 0; i < images.Length; i++)
		{
			if (images[i] != null)
			{
				ImageUtil.resize(ref images[i]!, scale);
				string text = AsciiScale.convertImage(ref images[i]!);
				attachments.AddLast(ToAttachment(text));
			}
		}

		// Respond with all images
		if (attachments.Count != 0)
			await FollowupWithFilesAsync(attachments);
		else
			await ModifyOriginalResponseAsync(msg => msg.Content = "Maybe add an image next time..");
	}

	private static FileAttachment ToAttachment(string imageText, string name = "unknown")
	{
		// Properly set file extension
		name = System.IO.Path.ChangeExtension(name, ".txt");

		// Convert img to file attachment
		MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(imageText));
		FileAttachment imageFile = new FileAttachment(ms, name);
		return imageFile;
	}
}