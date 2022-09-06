using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MongoDB.Driver;
using MongoDB.Bson;

public class SimpleModule : InteractionModuleBase
{
	private static Random randy = new Random(69420666);
	private MongoClient mc;

	public SimpleModule(MongoClient mc)
	{
		this.mc = mc;
	}

	[SlashCommand("say", "Repeats what you say")]
	public async Task echo(string input)
	{
		await RespondAsync(input);
	}

	[SlashCommand("mystery", "oooOOOoooohhHHHH mysterious button!!!")]
	public async Task button()
	{
		var cb = new ComponentBuilder()
		.WithButton(emote: new Emoji("❓"), customId: "boop");
		await RespondAsync(components: cb.Build());
	}

	[ComponentInteraction("boop")]
	public async Task buttonBoop()
	{
		var ctx = (SocketInteractionContext<SocketMessageComponent>)Context;

		// Connect to database + collection
		IMongoDatabase db = mc.GetDatabase("beepboop");
		IMongoCollection<BsonDocument> quotes = db.GetCollection<BsonDocument>("quotes");

		// Filter for target user
		FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("_id", $"{ctx.User.Id}");
		BsonDocument custom = quotes.Find(filter).FirstOrDefault();


		await ctx.Interaction.UpdateAsync((msg) =>
		{
			// Send message response or a custom one, if available
			msg.Content = custom == null ? "Get booped retard" : custom.GetElement("response").Value.ToString();
			msg.Components = new ComponentBuilder().Build();
		});
	}

	private static string[] biggyQuoteList = File.ReadAllLines("res/biggyquotes.txt");
	[SlashCommand("biggyquotes", "Crazy biggy quotes?!?!")]
	public async Task biggyQuote()
	{
		int index = randy.Next(biggyQuoteList.Length);
		await RespondAsync(biggyQuoteList[index]);
	}
}