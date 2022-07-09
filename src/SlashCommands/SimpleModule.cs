using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class SimpleModule : InteractionModuleBase {
	[SlashCommand("echo", "Testing slash command")]
	public async Task echo(string input) {
		await RespondAsync(input);
	}
}