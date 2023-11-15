using System;
using Discord.WebSocket;
using MongoDB.Bson;

public static class Twitter
{

	public static async void TwitterAutoEmbed(SocketMessage originalMessage)
	{
		SocketUserMessage message = (SocketUserMessage)originalMessage;
		if (message == null)
			return;
		if (message.Author.IsBot)
			return;

		if (Uri.TryCreate(message.Content, UriKind.Absolute, out _))
		{
			var builder = new UriBuilder(message.Content);
			if (builder.Host != "x.com" && builder.Host != "twitter.com")
				return;
			builder.Host = "vxtwitter.com";
			await message.Channel.SendMessageAsync(builder.ToString());
		}
	}
}