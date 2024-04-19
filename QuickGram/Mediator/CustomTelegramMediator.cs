using QuickGram.BotCommands;

namespace QuickGram.Mediator;

public class CustomTelegramMediator : MediatR.Mediator
{
    private readonly IServiceProvider serviceProvider;

    public CustomTelegramMediator(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public CustomTelegramMediator(IServiceProvider serviceProvider, INotificationPublisher publisher) : base(serviceProvider, publisher)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task PublishCore(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        if (notification is not IBotCommandContext botCommandContext) return;

        var handler = handlerExecutors.Single(handlerExecutor =>
            handlerExecutor.HandlerInstance is TelegramCommandHandler commandHandler &&
            commandHandler.Command.Equals(botCommandContext.Command));

        var wrapperType = typeof(NotificationHandlerWrapperImpl<>).MakeGenericType(notification.GetType());
        var wrapper = Activator.CreateInstance(wrapperType, [handler!.HandlerCallback]) ?? throw new InvalidOperationException($"Could not create wrapper type for {notification.GetType()}");
        var handlerBase = (NotificationHandlerBase)wrapper;

        await handlerBase.Handle(notification, serviceProvider, cancellationToken);
    }
}