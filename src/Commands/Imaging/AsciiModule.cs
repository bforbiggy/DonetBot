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
	public async Task AsciiAsync(AsciiArgs? args = null) {
		// Perform image operations on all attached images
		foreach (Attachment attachment in Context.Message.Attachments.Where(a => a.ContentType.StartsWith("image/"))) {
			await ReplyWithMod(attachment.Url, args!);
		}
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(string target, AsciiArgs? args = null) {
		string? url = null;

		// Attempts to parse string as user id
		UInt64 userId = MentionUtils.TryParseUser(target, out userId) ? userId :
										UInt64.TryParse(target, out userId) ? userId : 0;

		// If string is an id, get avatar url
		if (userId != 0) {
			var user = await Context.Client.GetUserAsync(userId);
			url = user.GetAvatarUrl(size: 256);
		}
		// If string is not an id, it must be an url
		else if (Uri.IsWellFormedUriString(target, UriKind.Absolute)) {
			url = target;
		}

		// If no target user or image is found, notify user of error
		if (url == null) {
			await Context.Message.ReplyAsync("Could not find target user/image.");
			return;
		}

		await ReplyWithMod(url, args);
	}

	/// <summary>
	/// Retrieves image then performs all operations.
	/// This then posts the resulting image to the channel.
	/// </summary>
	public async Task ReplyWithMod(string url, AsciiArgs args = null!) {
		args = args ?? new AsciiArgs();
		Image<Rgba32> img = await ImageUtil.getModImage(url, args); // Gets images with generic operations
		FileAttachment imageFile = modImageFile(ref img, args); // Converts image to stream with filter-specific operations

		// Send the outputted image file
		await Context.Channel.SendFileAsync(imageFile);
	}

	private static FileAttachment modImageFile(ref Image<Rgba32> img, AsciiArgs args) {
		// Perform ascii specific image operations
		string text = AsciiScale.convertImage(ref img, args.Detailed);

		// Convert img to file attachment
		MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
		FileAttachment imageFile = new FileAttachment(ms, args.Name);
		return imageFile;
	}
}


// Ascii specific options
[NamedArgumentType]
public class AsciiArgs : ImageArgs {
	public override double Scale { get; set; } = 1.0;
	public override string Name { get; set; } = "unknown.txt";

	public bool Detailed { get; set; } = false;
}