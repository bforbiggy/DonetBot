namespace DonetBot;

public class Program{
    public static Task Main(string[] args){
        Bot bot = new Bot();
        return bot.run();
    }
}