namespace QuickGram.BotCommands;

public abstract class TelegramCommandHandler : INotificationHandler<IBotCommandContext>
{
    public abstract string Description { get; }

    public abstract string Command { get; }

    public virtual int Calls { get; protected set; }

    public virtual async Task Handle(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(notification.Command)) return;
        if (string.IsNullOrWhiteSpace(Command)) return;
        if (!string.Equals(notification.Command, Command)) return;

        Calls++;

        await Core(notification, cancellationToken).ConfigureAwait(false);

        if (await IsLastStage())
        {
            notification.SetNeedRefresh();
        }
    }

    protected abstract Task Core(IBotCommandContext notification, CancellationToken cancellationToken);

    public virtual ValueTask<bool> IsLastStage() => new(true);
}