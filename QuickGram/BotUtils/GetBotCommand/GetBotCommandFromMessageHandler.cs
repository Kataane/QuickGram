namespace QuickGram.BotUtils.GetBotCommand;

public class GetBotCommandFromMessageHandler : GetBotCommandHandler
{
    public override UpdateType Type => UpdateType.Message;

    protected internal override bool CanHandle(GetBotCommandNotification notification)
    {
        if (string.IsNullOrWhiteSpace(notification.Update.Message?.Text)) return false;
        if (notification.Update.Message?.Text.Length is > MaxMessageLength or < MinMessageLength) return false;

        if (notification.Update.Message?.Text[0] != '/') return false;

        var messageEntity = notification.Update.Message?.Entities?[0];
        if (messageEntity?.Type != MessageEntityType.BotCommand) return false;

        return true;
    }

    protected internal override void SetCommand(GetBotCommandNotification notification)
    {
        notification.Command = notification.Update.Message!.Text!;
    }
}