namespace DonetBot.Commands;

using Discord;
using Discord.Commands;
using DonetBot.config;
using image_filter_tools;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

public class GrayscaleModule : ModuleBase<SocketCommandContext> {
	[Command("grayscale")]
	[Summary("Grayscales an image.")]
	public async Task GrayscaleAsync() {
		// Perform image operations on all attached images
		foreach (Attachment attachment in Context.Message.Attachments.Where(a => a.ContentType.StartsWith("image/"))) {
			// Retrieve image from stream
			Stream s = await new HttpClient().GetStreamAsync(attachment.Url);
			Image<Rgba32> img = SixLabors.ImageSharp.Image.Load<Rgba32>(s);

			// Perform image operation and save to stream
			GrayScale.convertImage(ref img);
			MemoryStream ms = new MemoryStream();
			img.Save(ms, new PngEncoder());

			// Reply to message with image
			FileAttachment imageFile = new FileAttachment(ms, "unknown.png");
			await Context.Channel.SendFileAsync(imageFile);

			await ReplyAsync();
		}

		await ReplyAsync("Converted all attached images to ascii.");
	}
}

[NamedArgumentType]
public class GrayscaleArgs {
	public double Scale { get; set; }
	public bool Detailed { get; set; }
}