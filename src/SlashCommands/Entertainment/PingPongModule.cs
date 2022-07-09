using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();

	int score = 0;
	PPStatus status = PPStatus.PONG;

	[SlashCommand("pingpong", "Play ball mf")]
	public async Task PingPongHandler() {
		// Generate game
		ulong userId = Context.User.Id;
		var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: "returnpingpong");
		await RespondAsync($"{enumToString(status)}", components: cb.Build());
		var msg = await GetOriginalResponseAsync();

		do {
			SocketInteraction interaction = await InteractionUtility.WaitForInteractionAsync((BaseSocketClient)Context.Client, TimeSpan.FromSeconds(3), (ctx) => {
				return userId == ctx.User.Id;
			});


			// If interaction is null, the user dies due to being too slow
			if (interaction == null) {
				await msg.ModifyAsync((msg) => {
					msg.Content = $"slow ass mf\nScore:{score}";
					msg.Components = null;
				});
				break;
			}
			await interaction.DeferAsync();


			// Otherwise, check if a bomb was hit
			if (status == PPStatus.BOMB) {
				await msg.ModifyAsync((msg) => {
					msg.Content = $"SOMEONE TOOK AN L!??!?!\nScore:{score}";
					msg.Components = null;
				});

				break;
			}
			// Otherwise, check if a pong was hit
			else if (status == PPStatus.PONG) {
				// Register hit and increase score
				score++;

				// Reset to waiting for next hit
				status = PPStatus.WAITING;
				await ModifyOriginalResponseAsync((msg) => {
					msg.Content = $"{enumToString(status)}";
				});
				await Task.Delay(2 * 1000);

				// Load next bot serve & update status
				status = await loadNext(interaction);
			}

		} while (true);
	}

	private async Task<PPStatus> loadNext(SocketInteraction interaction) {
		PPStatus status;
		if (randy.Next(10) <= 2) {
			status = PPStatus.PONG;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
		}
		else {
			status = PPStatus.BOMB;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
		}

		return status;
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