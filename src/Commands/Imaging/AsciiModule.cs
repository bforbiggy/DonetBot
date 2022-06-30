namespace DonetBot.Commands;

using Discord;
using Discord.Commands;
using DonetBot.config;
using image_filter_tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text;

public class AsciiModule : ModuleBase<SocketCommandContext> {
	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync() {
		await AsciiAsync(new AsciiArgs());
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(AsciiArgs args) {
		// Perform image operations on all attached images
		foreach (Attachment attachment in Context.Message.Attachments.Where(a => a.ContentType.StartsWith("image/"))) {
			// Retrieve image from stream
			Stream s = await new HttpClient().GetStreamAsync(attachment.Url);
			Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(s);

			// Modify image and convert to stream
			MemoryStream ms = modImageStream(img, args);

			// Reply to message with image
			FileAttachment imageFile = new FileAttachment(ms, "unknown.txt");
			await Context.Channel.SendFileAsync(imageFile);
		}


		await ReplyAsync("Converted all attached images to ascii.");
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(string url) {
		await AsciiAsync(url, new AsciiArgs());
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(string url, AsciiArgs args) {
		// Retrieve image from stream
		Stream s = await new HttpClient().GetStreamAsync(url);
		Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(s);

		// Modify image and convert to stream
		MemoryStream ms = modImageStream(img, args);

		// Reply to message with image
		FileAttachment imageFile = new FileAttachment(ms, "unknown.txt");
		await Context.Channel.SendFileAsync(imageFile);


		await ReplyAsync("Converted all attached images to ascii.");
	}

	public MemoryStream modImageStream(Image<Rgba32> img, AsciiArgs args) {
		// Perform image operations
		Size size = new Size((int)(img.Width * args.Scale), (int)(img.Height * args.Scale));
		img.Mutate(accessor => accessor.Resize(size));
		char[,] text = AsciiScale.convertImage(ref img, args.Detailed);

		// Save image to stream
		StringBuilder sb = new StringBuilder();
		for (int y = 0; y < text.GetLength(0); y++) {
			for (int x = 0; x < text.GetLength(1); x++)
				sb.Append(text[y, x]);
			sb.AppendLine();
		}

		return new MemoryStream(ASCIIEncoding.Default.GetBytes(sb.ToString()));
	}
}

[NamedArgumentType]
public class AsciiArgs {
	public double Scale { get; set; } = 1.0;
	public bool Detailed { get; set; } = false;
}