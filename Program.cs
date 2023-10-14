using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => { });
builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();


app.MapPost("/donetbot/github", async context =>
{
	JsonDocument document = await JsonDocument.ParseAsync(context.Request.Body);
	Console.WriteLine(document.RootElement.GetRawText());
	await context.Response.WriteAsync("Gotcha baby girl");
});

app.Run();