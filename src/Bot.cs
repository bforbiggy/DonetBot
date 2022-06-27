namespace DonetBot;

using DonetBot.Commands;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

public class Bot
{
    private const string TOKEN = "OTc0Mzk0ODQ1MTY0NjI1OTcw.GbPlkZ.Je-2rK_5qmwogMmGgUnJdZ6xOqKM5px3xjliV4";

    private DiscordSocketClient client;

    private CommandService commands;
    private CommandHandler commandHandler;

    public Bot(){
        // Enable client + logging
        client = new DiscordSocketClient();
        client.Log += Log;

        // Enable command service
        commands = new CommandService();
        commandHandler = new CommandHandler(client, commands);
    }

    public async Task run(){
        // Login and start bot
        await client.LoginAsync(TokenType.Bot, TOKEN);
        await client.StartAsync();

        // Event hooks
        // NOTHING FOR NOW

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}