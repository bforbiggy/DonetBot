using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class SimpleModule : InteractionModuleBase {
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
}