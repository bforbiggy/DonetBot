using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;
using Sprache;

public class PingPongModule : InteractionModuleBase {
	private static Random randy = new Random();
	public InteractiveService Interactive { get; set; } = null!;

	// Game data
	int score = 0;
	PPStatus status = PPStatus.PONG;

	// Timer  values
	double timer;
	double afkLossTimer = 3;
	double bombSafeTimer = 3;
	double waitTimer = 1.3;


	[SlashCommand("pingpong", "Play ball mf")]
	public async Task PingPongHandler() {
		// Generate game
		timer = afkLossTimer;
		var cb = new ComponentBuilder()
			.WithButton(emote: new Emoji("🏓"), customId: "returnpingpong");
		await RespondAsync($"{enumToString(status)}", components: cb.Build());
		var originalMsg = await GetOriginalResponseAsync();

		do {
			// Waits for user interaction
			var press = await Interactive.NextMessageComponentAsync(ctx => {
				return ctx.Message.Id == originalMsg.Id && ctx.Message.Author.Id == originalMsg.Author.Id;
			}, timeout: TimeSpan.FromSeconds(timer));

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
					calculateDifficulty();
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
		int tempTimer = randy.Next((int)(waitTimer * 1000), (int)(waitTimer * 1.5 * 1000));
		await Task.Delay(tempTimer);

		// Generate pong serve
		if (randy.Next(10) <= 8) {
			status = PPStatus.PONG;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
			double multiplier = 1 + randy.NextDouble() % 0.3;
			timer = afkLossTimer;
		}
		// Generate bomb
		else {
			status = PPStatus.BOMB;
			await ModifyOriginalResponseAsync((msg) => {
				msg.Content = $"{enumToString(status)}";
			});
			timer = bombSafeTimer;
		}
	}

	private void calculateDifficulty() {
		afkLossTimer = 3.0331 + -0.551773 * Math.Log(score); // (1,3) (20, 1.5) (50, 0.7)
		bombSafeTimer = Math.Max(1, bombSafeTimer - 0.05);
		waitTimer = Math.Max(0.7, waitTimer - 0.02);
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