namespace DonetBot.Commands;

using Discord.Commands;
using DonetBot.config;

public class ConfigModule : ModuleBase<SocketCommandContext>
{
    [Command("config")]
    [Summary("Changes a specified configuration.")]
    public async Task ChangeConfigAsync([Summary("Name of the setting to change.")]string settingName, [Remainder][Summary("New setting value.")] string settingValue)
    {
        GuildSettings guildSettings = CommandHandler.guildsSettings[Context.Guild.Id];

        if(guildSettings.settings.ContainsKey(settingName)){
            guildSettings.settings[settingName] = settingValue;
            await Context.Message.AddReactionAsync(Discord.Emoji.Parse(":white_check_mark:"), null);
        }
        else{
            await Context.Message.AddReactionAsync(Discord.Emoji.Parse(":x:"), null);
        }
    }
}