using Discord;

namespace DonetBot.config;

public class GuildSettings{
    public Dictionary<string, string> DEFAULT_SETTINGS = new Dictionary<string, string>(){
        {"ai.answer.qna", "I am an intelligent question answering bot. If you ask me a factual question, I will give you the answer. If you ask me an opinionated question, I will tell you what I believe."}
    };

    // Guild these settings are associated with
    public IGuild guild;

    // Guild-wide settings
    public Dictionary<string, string> settings;

    // Guild-wide user settings
    public Dictionary<IUser, UserSettings> userSettings;

    public GuildSettings(IGuild guild){
        this.guild = guild;
        settings = new Dictionary<string, string>(DEFAULT_SETTINGS);
        userSettings = new Dictionary<IUser, UserSettings>();
    }
}