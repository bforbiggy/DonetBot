using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MongoDB.Driver;
using MongoDB.Bson;

[DefaultMemberPermissions(GuildPermission.UseApplicationCommands)]
public class SimpleModule : InteractionModuleBase
{
	private static Random randy;
	private MongoClient mc;

	public SimpleModule(Random randy, MongoClient mc)
	{
		this.randy = randy;
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
		IMongoCollection<BsonDocument> quotes = db.GetCollection<BsonDocument>("mystery");

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

	[SlashCommand("biggyquotes", "Crazy biggy quotes?!?!")]
	public async Task biggyQuote()
	{
		// Connect to database + collection
		IMongoDatabase db = mc.GetDatabase("beepboop");
		IMongoCollection<BsonDocument> quotes = db.GetCollection<BsonDocument>("quotes");

		// Pick random quote from database and respond with it
		BsonDocument sample = new BsonDocument {
			{ "$sample", new BsonDocument{{"size", 1}} }
		};
		var quote = quotes.Aggregate<BsonDocument>(new BsonDocument[] { sample }).First();
		await RespondAsync(quote.GetElement("quote").Value.ToString());
	}
}