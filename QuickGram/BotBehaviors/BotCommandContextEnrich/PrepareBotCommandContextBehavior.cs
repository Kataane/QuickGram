namespace QuickGram.BotBehaviors.BotCommandContextEnrich;

public class PrepareBotCommandContextBehavior<TRequest, TResponse>(IServiceProvider serviceProvider)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBotCommandContext
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var updateReceiver = serviceProvider.GetServices<IBotCommandContextEnrich>().SingleOrDefault(r => r.CanHandle(request));

        if (updateReceiver == null)
        {
            return await next();
        }

        var message = updateReceiver.GetMessage(request);
        var args = updateReceiver.GetArgs(request).ToList();

        request.AddMessage(message);
        request.AddArgs(args);

        return await next();
    }
}