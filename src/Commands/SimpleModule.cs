namespace DonetBot.Commands;

using Discord.Commands;

public class SimpleModule : ModuleBase<SocketCommandContext> {
	[Command("say")]
	[Summary("Repeats your message.")]
	public Task SayAsync([Remainder][Summary("The text to repeat")] string echo) {
		await Context.Message.DeleteAsync();
		return ReplyAsync(echo);
	}

	protected static Random randy = new Random(69420666);
	[Command("random")]
	[Summary("Returns an inclusive number between the lower and upper bound.")]
	public Task RandomAsync(int lower, int upper) {
		return ReplyAsync(randy.Next(lower, upper).ToString());
	}

	[Command("time")]
	[Summary("Returns the current unix timestamp")]
	public Task TimeAsync() {
		long time = Context.Message.Timestamp.ToUnixTimeSeconds();
		return ReplyAsync($"<t:{time}>");
	}
}