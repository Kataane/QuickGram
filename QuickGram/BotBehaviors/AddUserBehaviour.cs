namespace QuickGram.BotBehaviors;

public class AddUserBehaviour<TRequest, TResponse>(IDistributedCache cache) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBotCommandContext
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var value = await cache.GetStringAsync(request.Chat.Id.ToString(), cancellationToken);
        if (!string.IsNullOrWhiteSpace(value)) return await next();

        await cache.SetAsync(request.Chat.Id.ToString(), Encoding.ASCII.GetBytes(request.Chat.Id.ToString()), cancellationToken);

        return await next();
    }
}