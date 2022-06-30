namespace DonetBot.Commands;

using Discord.Commands;

public class SimpleModule : ModuleBase<SocketCommandContext> {
	[Command("say")]
	[Summary("Repeats your message.")]
	public Task SayAsync([Remainder][Summary("The text to repeat")] string echo) {
		Context.Message.DeleteAsync();
		return ReplyAsync(echo);
	}
}