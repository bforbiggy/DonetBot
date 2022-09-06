using System.ComponentModel;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

public class UtilityModule : InteractionModuleBase
{
	private static Random randy = new Random(69420666);

	[SlashCommand("random", "Returns a random number")]
	public async Task random([Summary(description: "Minimum value of number")] int lower = 0, [Summary(description: "Maximum value of number")] int upper = Int32.MaxValue)
	{
		int num = randy.Next(lower, upper);
		await RespondAsync(num.ToString());
	}

	[SlashCommand("time", "Returns the current time")]
	public async Task time()
	{
		long time = Context.Interaction.CreatedAt.ToUnixTimeSeconds();
		await RespondAsync($"<t:{time}>");
	}
}