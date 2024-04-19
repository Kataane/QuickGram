namespace QuickGram.BotBehaviors;

public class BotCommandCounterBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBotCommandContext
{
    private int counter;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        counter++;
        return await next();
    }
}