using Discord;

namespace DonetBot.config;

public class UserSettings{
    // User-specific settings
    public Dictionary<string, string> settings;

    public UserSettings(){
        settings = new Dictionary<string, string>();
    }
}