namespace QuickGram.BotBehaviors.BotCommandContextEnrich;

public class MessageUpdateReceiver : IBotCommandContextEnrich
{
    public bool CanHandle(IBotCommandContext notification)
    {
        return notification.Update.Type == UpdateType.Message;
    }

    public Message GetMessage(IBotCommandContext notification)
    {
        ArgumentNullException.ThrowIfNull(notification.Update.Message);
        return notification.Update.Message;
    }

    public IEnumerable<string> GetArgs(IBotCommandContext notification)
    {
        var strings = notification.Update.Message!.Text!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return strings.Length > 0 ? strings[1..] : [];
    }
}