namespace DonetBot.Commands;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DonetBot.config;

public class CommandHandler
{
    private const char COMMAND_PREFIX = '!';

    private readonly DiscordSocketClient client;
    private readonly CommandService commands;

    public static Dictionary<ulong, GuildSettings> guildsSettings = new Dictionary<ulong, GuildSettings>();

    // Retrieve client and CommandService instance via ctor
    public CommandHandler(DiscordSocketClient client, CommandService commands)
    {
        this.commands = commands;
        this.client = client;

        _ = InstallCommandsAsync();
    }

    public async Task InstallCommandsAsync()
    {
        // Hook the MessageReceived event into our command handler
        client.MessageReceived += HandleCommandAsync;

        // Load all modules in the command service
        await commands.AddModuleAsync(typeof(SimpleModule), null);
        await commands.AddModuleAsync(typeof(GPTModule), null);
        await commands.AddModuleAsync(typeof(ConfigModule), null);
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        // Don't process the command if it was a system or bot message
        var message = messageParam as SocketUserMessage;
        if (message == null || message.Author.IsBot) return;

        // Determine if the message is a command based on the prefix and make sure no bots trigger commands
        int argPos = 0;
        if (!message.HasCharPrefix(COMMAND_PREFIX, ref argPos))
            return;

        // Generate default guild permissions if unavailable
        SocketGuildChannel channel = (SocketGuildChannel)message.Channel;
        SocketGuild guild = channel!.Guild;
        if (!guildsSettings.ContainsKey(guild.Id)){
            guildsSettings[guild.Id] = new GuildSettings(guild);
        }


        // Create a WebSocket context from message and 
        // inject service provider for preconditional check for commands
        SocketCommandContext context = new SocketCommandContext(client, message);
        await commands.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: null);
    }
}