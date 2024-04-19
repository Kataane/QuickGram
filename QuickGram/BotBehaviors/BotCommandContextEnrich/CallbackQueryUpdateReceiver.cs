namespace QuickGram.BotBehaviors.BotCommandContextEnrich;

public class CallbackQueryUpdateReceiver : IBotCommandContextEnrich
{
    public bool CanHandle(IBotCommandContext notification)
    {
        return notification.Update.Type == UpdateType.CallbackQuery;
    }

    public Message GetMessage(IBotCommandContext notification)
    {
        ArgumentNullException.ThrowIfNull(notification.Update.CallbackQuery?.Message!);
        return notification.Update.CallbackQuery.Message;
    }

    public IEnumerable<string> GetArgs(IBotCommandContext notification)
    {
        var strings = notification.Update.CallbackQuery!.Data!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return strings.Length > 0 ? strings[1..] : [];
    }
}