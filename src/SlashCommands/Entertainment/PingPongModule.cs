using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();
	public InteractiveService? Interactive { get; set; }

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
			var press = await Interactive.NextMessageComponentAsync(ctx => ctx.Message.Id == msg.Id, timeout: TimeSpan.FromSeconds(3));

			// If interaction is null, the user dies due to being too slow
			if (press.IsTimeout) {
				await msg.ModifyAsync((msg) => {
					msg.Content = $"slow ass mf\nScore:{score}";
					msg.Components = null;
				});
				break;
			}
			SocketMessageComponent result = press.Value!;
			await result.DeferAsync();


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
				status = await loadNext(result);
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