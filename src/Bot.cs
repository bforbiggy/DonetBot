namespace DonetBot;

using DotNetEnv;
using DonetBot.Commands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Fergun.Interactive;
using Microsoft.Extensions.DependencyInjection;

public class Bot {
	private String token;

	private DiscordSocketClient client;
	private CommandService commandService;
	private InteractionService interactionService;

	private ServiceProvider services = new ServiceCollection()
				.AddSingleton(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose })
				.AddSingleton(new CommandServiceConfig { LogLevel = LogSeverity.Verbose })
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<CommandService>()
				.AddSingleton<CommandHandler>()
				.AddSingleton<InteractionService>()
				.AddSingleton<InteractiveService>()
				.AddSingleton<InteractionHandler>()
				.BuildServiceProvider();

	public Bot(String token) {
		this.token = token;

		// Retrieve specific services
		client = services.GetRequiredService<DiscordSocketClient>();
		commandService = services.GetRequiredService<CommandService>();
		interactionService = services.GetRequiredService<InteractionService>();

		// Enable logging
		client.Log += Log;
		commandService.Log += Log;
		interactionService.Log += Log;
	}

	public async Task run() {
		// Login and start bot
		await client.LoginAsync(TokenType.Bot, token);
		await client.StartAsync();

		// Initialize interaction/command handlers
		await services.GetRequiredService<CommandHandler>().InstallCommandsAsync();
		services.GetRequiredService<InteractionHandler>().InstallCommandsAsync();

		// Event hooks
		// client.MessageReceived += (msg) => {
		// 	return Task.CompletedTask;
		// };

		// Prevent bot shutdown until program exit
		await Task.Delay(Timeout.Infinite);
	}

	private Task Log(LogMessage msg) {
		Console.WriteLine(msg.ToString());
		return Task.CompletedTask;
	}

	public static Task Main(string[] args) {
		Env.Load();
		Bot bot = new Bot(Env.GetString("TOKEN"));
		return bot.run();
	}
}