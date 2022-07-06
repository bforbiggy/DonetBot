namespace DonetBot.Commands;

using Discord;
using Discord.Commands;
using image_filter_tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

public class AsciiModule : ModuleBase<SocketCommandContext> {
	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(ulong id, AsciiArgs args = null!) {
		var user = await Context.Client.GetUserAsync(id);
		string url = user.GetAvatarUrl(size: 256);
		await AsciiAsync(url, args);
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(AsciiArgs args = null!) {
		// Perform image operations on all attached images
		foreach (Attachment attachment in Context.Message.Attachments.Where(a => a.ContentType.StartsWith("image/"))) {
			await AsciiAsync(attachment.Url, args);
		}
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	[Priority(0)]
	public async Task AsciiAsync(string url, AsciiArgs? args = null) {
		args = args ?? new AsciiArgs();

		// Retrieve image from stream
		Stream s = await new HttpClient().GetStreamAsync(url);
		Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(s);

		// Modify image and convert to stream
		MemoryStream ms = modImageStream(img, args);

		// Reply to message with image
		FileAttachment imageFile = new FileAttachment(ms, "unknown.txt");
		await Context.Channel.SendFileAsync(imageFile);
	}

	public MemoryStream modImageStream(Image<Rgba32> img, AsciiArgs args) {
		// Perform image operations
		Size size = new Size((int)(img.Width * args.Scale), (int)(img.Height * args.Scale));
		img.Mutate(accessor => accessor.Resize(size));
		string text = AsciiScale.convertImage(ref img, args.Detailed);

		// Write output to memory stream
		return new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
	}
}

[NamedArgumentType]
public class AsciiArgs {
	public double Scale { get; set; } = 1.0;
	public bool Detailed { get; set; } = false;
}