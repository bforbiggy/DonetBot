using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Interactions;
using Fergun.Interactive;
using Microsoft.Extensions.DependencyInjection;
public class InteractionHandler {
	public readonly DiscordSocketClient client;
	public readonly InteractionService interactions;
	private readonly IServiceProvider services;


	public InteractionHandler(IServiceProvider services) {
		this.services = services;
		this.client = services.GetRequiredService<DiscordSocketClient>();
		this.interactions = services.GetRequiredService<InteractionService>();
	}

	public void InstallCommandsAsync() {
		// Add interaction modules
		client.Ready += async () => {
			await interactions.AddModulesAsync(assembly: System.Reflection.Assembly.GetEntryAssembly(), services: services);
			await interactions.RegisterCommandsToGuildAsync(933828271798370314);
		};

		client.SlashCommandExecuted += async (interaction) => {
			var ctx = new SocketInteractionContext(client, interaction);
			await interactions.ExecuteCommandAsync(ctx, services);
		};

		client.ButtonExecuted += async (interaction) => {
			var ctx = new SocketInteractionContext<SocketMessageComponent>(client, interaction);
			await interactions.ExecuteCommandAsync(ctx, services);
		};
	}
}