using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();

	[SlashCommand("pingpong", "Play ball mf")]
	public async Task PingPongHandler() {
		// Generate game
		int score = 0;
		PPStatus status = PPStatus.PONG;
		ulong userId = Context.User.Id;
		var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: "returnpingpong");
		await RespondAsync($"{enumToString(status)}", components: cb.Build());

		do {
			SocketInteraction interaction = await InteractionUtility.WaitForInteractionAsync((BaseSocketClient)Context.Client, TimeSpan.FromSeconds(3), (ctx) => {
				return userId == ctx.User.Id;
			});

			// If interaction is null, the user dies due to being too slow
			if (interaction == null) {
				await ModifyOriginalResponseAsync((msg) => {
					msg.Content = $"slow ass mf\nScore:{score}";
				});
				break;
			}
			// Otherwise, check if a bomb was hit
			else if (status == PPStatus.BOMB) {
				await ModifyOriginalResponseAsync((msg) => {
					msg.Content = $"SOMEONE TOOK AN L!??!?!\nScore:{score}";
				});
				break;
			}
			// Otherwise, check if a pong was hit
			else if (status == PPStatus.PONG) {
				// Register hit and increase score
				score++;
				await interaction.DeferAsync();

				// Reset to waiting for next hit
				status = PPStatus.WAITING;
				await ModifyOriginalResponseAsync((msg) => {
					msg.Content = $"{enumToString(status)}";
				});

				// After some time, generate serve/bomb
				await Task.Delay(1 * 1000);
				status = PPStatus.PONG;
				await ModifyOriginalResponseAsync((msg) => {
					msg.Content = $"{enumToString(status)}";
				});
			}
		} while (true);
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