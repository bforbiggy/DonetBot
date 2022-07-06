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
	public async Task AsciiAsync(ImageArgs? args = null) {
		// Perform image operations on all attached images
		foreach (Attachment attachment in Context.Message.Attachments.Where(a => a.ContentType.StartsWith("image/"))) {
			await ReplyWithMod(attachment.Url, args);
		}
	}

	[Command("ascii")]
	[Summary("Converts an image to ascii.")]
	public async Task AsciiAsync(string target, ImageArgs? args = null) {
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
	public async Task ReplyWithMod(string url, ImageArgs? args = null) {
		args = args ?? new ImageArgs();
		Image<Rgba32> img = await ImageUtil.getModImage(url, args); // Gets images with generic operations
		FileAttachment imageFile = modImageFile(ref img, args); // Converts image to stream with filter-specific operations

		// Send the outputted image file
		await Context.Channel.SendFileAsync(imageFile);
	}

	private static FileAttachment modImageFile(ref Image<Rgba32> img, ImageArgs args) {
		// Perform ascii specific image operations
		args.Name = System.IO.Path.ChangeExtension(args.Name, ".txt");
		string text = AsciiScale.convertImage(ref img, args.Detailed);

		// Convert img to file attachment
		MemoryStream ms = new MemoryStream(ASCIIEncoding.Default.GetBytes(text));
		FileAttachment imageFile = new FileAttachment(ms, args.Name);
		return imageFile;
	}
}