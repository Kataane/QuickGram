namespace QuickGram.BotUtils.GetBotCommand;

public abstract class GetBotCommandHandler : INotificationHandler<GetBotCommandNotification>
{
    public const int MaxMessageLength = 40;
    public const int MinMessageLength = 2;

    public abstract UpdateType Type { get; }

    public Task Handle(GetBotCommandNotification command, CancellationToken cancellationToken)
    {
        if (command.Update.Type != Type) return Task.CompletedTask;
        if (!CanHandle(command)) return Task.CompletedTask;

        SetCommand(command);
        return Task.CompletedTask;
    }

    protected internal abstract bool CanHandle(GetBotCommandNotification notification);

    protected internal abstract void SetCommand(GetBotCommandNotification notification);
}