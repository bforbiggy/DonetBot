using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.WebSocket;
public class InteractionHandler {
	private readonly DiscordSocketClient client;
	private readonly InteractionService interactions;

	public InteractionHandler(DiscordSocketClient client, InteractionService interactions) {
		this.client = client;
		this.interactions = interactions;

		InstallCommandsAsync();
	}

	private void InstallCommandsAsync() {
		// Add interaction modules
		client.Ready += async () => {
			await interactions.AddModulesAsync(assembly: System.Reflection.Assembly.GetEntryAssembly(), services: null);
			await interactions.RegisterCommandsToGuildAsync(933828271798370314);
		};

		client.SlashCommandExecuted += async (interaction) => {
			var ctx = new SocketInteractionContext(client, interaction);
			await interactions.ExecuteCommandAsync(ctx, null);
		};

		client.ButtonExecuted += async (interaction) => {
			var ctx = new SocketInteractionContext<SocketMessageComponent>(client, interaction);
			await interactions.ExecuteCommandAsync(ctx, null);
		};
	}
}