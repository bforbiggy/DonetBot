using System.ComponentModel;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

[DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
public class UtilityModule : InteractionModuleBase
{
	private static Random randy = new Random(69420666);

	[SlashCommand("random", "Returns a random number")]
	public async Task random(
		[Summary(description: "Minimum value of number")] int lower = 0,
		[Summary(description: "Maximum value of number")] int upper = Int32.MaxValue)
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

	[SlashCommand("timer", "Sets a timer.")]
	public async Task timer(
		int days = 0,
		int hours = 0,
		int minutes = 0,
		int seconds = 0,
		string message = "!!!!TIMER DONE!!!!")
	{
		// Set alarm and finish command response
		TimeSpan time = TimeSpan.FromDays(days) + TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds);
		await RespondAsync($"Timer for {time:g} set.");

		// Wait then send alarm!!!
		Thread.Sleep((int)time.TotalMilliseconds);
		var ctx = (SocketInteractionContext)Context;
		await ReplyAsync($"{ctx.User.Mention} {message}");
	}
}