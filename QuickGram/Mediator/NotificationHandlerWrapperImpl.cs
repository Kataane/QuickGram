namespace QuickGram.Mediator;

public class NotificationHandlerWrapperImpl<TRequest>(Func<TRequest, CancellationToken, Task> handlerCallback) : NotificationHandlerBase
    where TRequest : INotification
{
    public Task Handle(TRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        return serviceProvider
            .GetServices<IPipelineBehavior<TRequest, Unit>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<Unit>)Handler,
                (next, pipeline) => () => pipeline.Handle(request, next, cancellationToken))();

        async Task<Unit> Handler()
        {
            await handlerCallback(request, cancellationToken);
            return Unit.Value;
        }
    }

    public override Task
        Handle(object request, IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        Handle((TRequest)request, serviceProvider, cancellationToken);
}