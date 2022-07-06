namespace DonetBot.Commands;

using Discord.Commands;

public class SimpleModule : ModuleBase<SocketCommandContext> {
	[Command("say")]
	[Summary("Repeats your message.")]
	public async Task SayAsync([Remainder][Summary("The text to repeat")] string echo) {
		_ = Context.Message.DeleteAsync();
		await ReplyAsync(echo);
	}

	protected static Random randy = new Random(69420666);
	[Command("random")]
	[Summary("Returns an inclusive number between the lower and upper bound.")]
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
