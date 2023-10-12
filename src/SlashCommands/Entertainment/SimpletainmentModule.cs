using Discord;
using Discord.Commands.Builders;
using Discord.Interactions;
using Discord.WebSocket;
using Fergun.Interactive;
using Newtonsoft.Json.Linq;

[DefaultMemberPermissions(GuildPermission.UseApplicationCommands | GuildPermission.SendMessages)]
public class SimpletainmentModule : InteractionModuleBase
{
	private Random randy;
	private HttpClient client;

	public SimpletainmentModule(Random randy, HttpClient client)
	{
		this.randy = randy;
		this.client = client;
	}


	private static long lastGamble = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 1;
	[SlashCommand("gamble", "Gamble for characters")]
	public async Task gamble()
	{
		// Ensure request does not bypass rate limit
		long current = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		if (current - lastGamble <= 0)
		{
			await RespondAsync("The bot is limited at one gamble/sec globally!!!!");
			return;
		}
		lastGamble = current;

		// Retrieve request from target
		HttpResponseMessage response = await client.GetAsync($"https://www.animecharactersdatabase.com/api_series_characters.php?character_id={randy.Next(133724)}");
		response.EnsureSuccessStatusCode(); // Switch to response.IsSuccessStatusCode
		string responseBody = await response.Content.ReadAsStringAsync();

		// Post-data processing + replying
		JObject json = JObject.Parse(responseBody);
		string name = json.GetValue("name")!.ToString();
		string image = json.GetValue("character_image")!.ToString();
		await RespondAsync($"You got {name}!!!!\n{image}");
	}
}