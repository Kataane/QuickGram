namespace QuickGram.BotUtils.GetBotCommand;

public class GetCommandFromCallbackQueryHandler : GetBotCommandHandler
{
    public override UpdateType Type => UpdateType.CallbackQuery;

    protected internal override void SetCommand(GetBotCommandNotification notification)
    {
        var strings = notification.Update.CallbackQuery!.Data!.Split(" ");
        notification.Command = strings[0];
    }

    protected internal override bool CanHandle(GetBotCommandNotification notification)
    {
        if (string.IsNullOrWhiteSpace(notification.Update.CallbackQuery?.Data)) return false;
        if (notification.Update.CallbackQuery.Data[0] != '/') return false;

        return true;
    }
}