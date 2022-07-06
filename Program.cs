namespace DonetBot;

using DotNetEnv;

public class Program {
	public static Task Main(string[] args) {
		Env.Load();

		Bot bot = new Bot(Env.GetString("TOKEN"));
		return bot.run();
	}
}
