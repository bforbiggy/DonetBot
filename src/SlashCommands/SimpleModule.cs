using System.ComponentModel;
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


		// Special kahoko case
		if (ctx.User.Id == 881128002497421352) {
			await ctx.Interaction.UpdateAsync((msg) => {
				msg.Content = "uwaaah~~ please bully me kahoko 🥵";
				msg.Components = new ComponentBuilder().Build();
			});
			return;
		}
		// Special liv case
		else if (ctx.User.Id == 870651943717044254) {
			await ctx.Interaction.UpdateAsync((msg) => {
				msg.Content = "Liv is an emotional manipulative bitch!";
				msg.Components = new ComponentBuilder().Build();
			});
			return;
		}
		// Special matthew case
		else if (ctx.User.Id == 302511455175966720) {
			await ctx.Interaction.UpdateAsync((msg) => {
				msg.Content = "uwu matthew zamners!";
				msg.Components = new ComponentBuilder().Build();
			});
			return;
		}

		await ctx.Interaction.UpdateAsync((msg) => {
			msg.Content = "Get booped retard";
			msg.Components = new ComponentBuilder().Build();
		});
	}

	private static string[] biggyQuoteList = File.ReadAllLines("res/biggyquotes.txt");
	[SlashCommand("biggyquotes", "Crazy biggy quotes?!?!")]
	public async Task biggyQuote() {
		int index = randy.Next(biggyQuoteList.Length);
		await RespondAsync(biggyQuoteList[index]);
	}
}