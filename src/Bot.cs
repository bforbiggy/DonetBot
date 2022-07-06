namespace DonetBot;

using DonetBot.Commands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class Bot {
	private String token;

	private DiscordSocketClient client;

	private CommandService commands;
	private CommandHandler commandHandler;

	public Bot(String token) {
		this.token = token;

		// Enable client + logging
		client = new DiscordSocketClient();
		client.Log += Log;

		// Enable command service
		commands = new CommandService();
		commandHandler = new CommandHandler(client, commands);
	}

	public async Task run() {
		// Login and start bot
		await client.LoginAsync(TokenType.Bot, token);
		await client.StartAsync();

		// Event hooks
		// client.MessageReceived += (msg) => {
		// 	return Task.CompletedTask;
		// };

		// Block this task until the program is closed.
		await Task.Delay(-1);
	}

	private Task Log(LogMessage msg) {
		Console.WriteLine(msg.ToString());
		return Task.CompletedTask;
	}
}