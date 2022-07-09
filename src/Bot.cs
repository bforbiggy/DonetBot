namespace DonetBot;

using DonetBot.Commands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Microsoft.VisualBasic;

public class Bot {
	private String token;

	private DiscordSocketClient client;

	private CommandService commands;
	private CommandHandler commandHandler;

	private InteractionService interactions;
	private InteractionHandler interactionHandler;

	public Bot(String token) {
		this.token = token;

		// Enable client + logging
		client = new DiscordSocketClient();
		client.Log += Log;

		// Enable command service
		commands = new CommandService();
		commands.Log += Log;
		commandHandler = new CommandHandler(client, commands);

		// Enable interaction service
		interactions = new InteractionService(client);
		interactions.Log += Log;
		interactionHandler = new InteractionHandler(client, interactions);
	}

	public async Task run() {
		// Login and start bot
		await client.LoginAsync(TokenType.Bot, token);
		await client.StartAsync();

		// Event hooks
		// client.MessageReceived += (msg) => {
		// 	return Task.CompletedTask;
		// };

		// Prevent bot shutdown until program exit
		await Task.Delay(-1);
	}

	private Task Log(LogMessage msg) {
		Console.WriteLine(msg.ToString());
		return Task.CompletedTask;
	}
}