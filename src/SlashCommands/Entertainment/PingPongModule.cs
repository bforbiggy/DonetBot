using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();

	[SlashCommand("pingpong", "Play ball mf")]
	public async Task PingPongHandler() {
		// Generate game
		PPStatus status = PPStatus.WAITING;
		var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: $"pingpongserve:{(int)status},{0}");
		await RespondAsync($"Score:0\n{enumToString(status)}", components: cb.Build());

		// Start serving up that pong
		await loadNextAsync(Context.Interaction);
	}

	[ComponentInteraction("pingpongserve:*,*")]
	public async Task Play(string typeStr, string scoreStr) {
		PPStatus status = (PPStatus)Int32.Parse(typeStr);
		int score = Int32.Parse(scoreStr);

		Console.WriteLine($"Custom id: pingpongserve:{typeStr},{scoreStr}");

		// If we got to serve the pong back
		if (status == PPStatus.PONG) {
			score++;

			// Set status to waiting..
			await Context.Interaction.ModifyOriginalResponseAsync((msg) => {
				var cb = new ComponentBuilder()
				.WithButton(emote: new Emoji("🏓"), customId: $"pingpongserve:{(int)status},{0}");
				msg.Content = $"Score:{score}\n{enumToString(PPStatus.WAITING)}";
				msg.Components = cb.Build();
			});



			// Load next pong
			await loadNextAsync(Context.Interaction, score);
		}
	}

	private async Task loadNextAsync(IDiscordInteraction context, int score = 0) {
		// Wait a bit before serving pong/bomb
		int time = 3;
		PPStatus status = PPStatus.PONG;
		await Task.Delay(time * 1000);

		// Update message with pong/bomb
		await context.ModifyOriginalResponseAsync((msg) => {
			var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: $"pingpongserve:{(int)status},{0}");
			msg.Content = $"Score:{score}\n{enumToString(status)}";
			msg.Components = cb.Build();
		});
	}

	private static string enumToString(PPStatus status) {
		if (status == PPStatus.WAITING)
			return "...";
		else if (status == PPStatus.PONG)
			return "🏓";
		else if (status == PPStatus.BOMB)
			return "💣";
		return "Internal server error.";
	}
}

enum PPStatus {
	WAITING = 0,
	PONG = 1,
	BOMB = 2
}