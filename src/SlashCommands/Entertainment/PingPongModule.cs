using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;
using Sprache;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();
	public InteractiveService Interactive { get; set; } = null!;

	int score = 0;
	double timer = 3;
	PPStatus status = PPStatus.PONG;


	[SlashCommand("pingpong", "Play ball mf")]
	public async Task PingPongHandler() {
		// Generate game
		ulong userId = Context.User.Id;
		var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: "returnpingpong");
		await RespondAsync($"{enumToString(status)}", components: cb.Build());
		var originalMsg = await GetOriginalResponseAsync();

		do {
			// Waits for user interaction
			var press = await Interactive.NextMessageComponentAsync(ctx => ctx.Message.Id == originalMsg.Id, timeout: TimeSpan.FromSeconds(timer));

			// If timer runs out, we determine outcome based on status
			if (press.IsTimeout) {
				// If it's a pong timeout, we create an afk lose
				if (status == PPStatus.PONG) {
					await ModifyOriginalResponseAsync((msg) => {
						msg.Content = $"slow ass mf\nScore:{score}";
						msg.Components = new ComponentBuilder().Build();
					});
					break;
				}
				// If it's a bomb timeout, generate next serve
				else if (status == PPStatus.BOMB) {
					await loadNext();
					continue;
				}
			}

			// When button is pressed, determine outcome based on status
			if (press.IsSuccess) {
				// Acknowledge the interaction
				SocketMessageComponent result = press.Value;
				await result.DeferAsync();

				// If a pong was hit, serve next pong
				if (status == PPStatus.PONG) {
					score++;
					await loadNext();
				}
				// If a bomb was hit, lose the game
				else if (status == PPStatus.BOMB) {
					await ModifyOriginalResponseAsync((msg) => {
						msg.Content = $"SOMEONE TOOK AN L!??!?!\nScore:{score}";
						msg.Components = new ComponentBuilder().Build();
					});
					break;
				}
			}
		} while (true);
	}

	private async Task loadNext() {
		// Reset to waiting for next hit
		status = PPStatus.WAITING;
		await ModifyOriginalResponseAsync((msg) => {
			msg.Content = $"{enumToString(status)}";
		});
		await Task.Delay(2 * 1000);

		// Generate pong serve
		if (randy.Next(10) <= 5) {
			status = PPStatus.PONG;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
		}
		// Generate bomb
		else {
			status = PPStatus.BOMB;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
		}
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