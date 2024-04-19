namespace QuickGram.Mediator;

public abstract class NotificationHandlerBase
{
    public abstract Task Handle(object request, IServiceProvider serviceProvider,
        CancellationToken cancellationToken);
}