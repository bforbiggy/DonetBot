namespace DonetBot.Commands;

using Discord.Commands;

public class SimpleModule : ModuleBase<SocketCommandContext> {
	[Command("say")]
	[Summary("Repeats your message.")]
	public async Task SayAsync([Remainder][Summary("The text to repeat")] string repeat) {
		_ = Context.Message.DeleteAsync();
		await ReplyAsync(repeat);
	}

	protected static Random randy = new Random(69420666);
	[Command("random")]
	[Summary("Returns an random number within bounds. (exclusive upper bound)")]
	public async Task RandomAsync(int upper) {
		await ReplyAsync(randy.Next(upper).ToString());
	}

	[Command("random")]
	[Summary("Returns an random number within bounds. (inclusive)")]
	public async Task RandomAsync(int lower, int upper) {
		await ReplyAsync(randy.Next(lower, upper).ToString());
	}

	[Command("time")]
	[Summary("Returns the current unix timestamp")]
	public async Task TimeAsync() {
		long time = Context.Message.Timestamp.ToUnixTimeSeconds();
		await ReplyAsync($"<t:{time}>");
	}
}
