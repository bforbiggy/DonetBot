using Discord.Commands;

[NamedArgumentType]
public class ImageArgs {
	public virtual double Scale { get; set; } = 1.0;
	public virtual string Name { get; set; } = "unknown.png";
}