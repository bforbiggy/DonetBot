namespace DonetBot.Commands;

using DotNetEnv;
using Discord.Commands;
using DonetBot.config;
using OpenAI_API;

public class GPTModule : ModuleBase<SocketCommandContext>
{

	private static string GPT_KEY = Env.GetString("GPT_KEY");
	private static OpenAIAPI api = new OpenAIAPI(engine: Engine.Default, apiKeys: GPT_KEY);

	[Command("complete")]
	[Summary("Provides AI completion of any chunk of text.")]
	public async Task CompleteAsync([Remainder][Summary("User inputted text")] string input)
	{
		// Creates GPT-3 request with appropriate parameters
		CompletionRequest request = new CompletionRequest(input, temperature: 0.1);
		request.MaxTokens = 100;
		request.StopSequence = "\n";

		// Get response from API and output to user
		CompletionResult response = await api.Completions.CreateCompletionAsync(request);
		await ReplyAsync(input + response.ToString());
	}


	[Command("answer")]
	[Summary("Provides AI generated answer for any question.")]
	public async Task AnswerAsync([Remainder][Summary("User inputted question")] string question)
	{
		// Formats user input for GPT-3 completion
		GuildSettings guildSettings = CommandHandler.guildsSettings[Context.Guild.Id];
		string formattedQuestion = $"Q: {question}";
		string requestString = $"{guildSettings.settings["ai.answer.qna"]}\n\n{formattedQuestion}";

		// Creates GPT-3 request with appropriate parameters
		CompletionRequest request = new CompletionRequest(requestString, temperature: 0.2);
		request.MaxTokens = 100;
		request.StopSequence = "Q:";

		// Get response from API and output to user
		CompletionResult response = await api.Completions.CreateCompletionAsync(request);
		await ReplyAsync(formattedQuestion + response.ToString());
	}
}