using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class SimpleModule : InteractionModuleBase {
	private static Random randy = new Random();

	[SlashCommand("echo", "Testing slash command")]
	public async Task echo(string input) {
		await RespondAsync(input);
	}

	[SlashCommand("mystery", "oooh what does this do")]
	public async Task button() {
		var cb = new ComponentBuilder()
		.WithButton(emote: new Emoji("❓"), customId: "boop");
		await RespondAsync(components: cb.Build());
	}

	[ComponentInteraction("boop")]
	public async Task buttonBoop() {
		var ctx = (SocketInteractionContext<SocketMessageComponent>)Context;
		await ctx.Interaction.UpdateAsync((msg) => {
			msg.Content = "Get booped retard";
			msg.Components = null;
		});
	}

	private static string[] biggyQuoteList = File.ReadAllLines("res/biggyquotes.txt");
	[SlashCommand("biggyquotes", "Crazy biggy quotes?!?!")]
	public async Task biggyQuote() {
		int index = randy.Next(biggyQuoteList.Length);
		await RespondAsync(biggyQuoteList[index]);
	}
}