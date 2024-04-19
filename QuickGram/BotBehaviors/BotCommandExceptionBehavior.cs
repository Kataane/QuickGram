namespace QuickGram.BotBehaviors;

public class BotCommandExceptionBehavior<TRequest, TResponse>(ITelegramBotClient botClient)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBotCommandContext
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await botClient.SendTextMessageAsync(request.Chat, $"При выполнении комманды: '{request.Command}', произошла ошибка: {e.Message}", cancellationToken: cancellationToken);
        }

        return default!;
    }
}