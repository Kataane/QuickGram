namespace QuickGram.BotUtils.GetBotCommand;

public record GetBotCommandNotification : INotification
{
    public required Update Update { get; set; }
    public string Command { get; set; } = string.Empty;
}