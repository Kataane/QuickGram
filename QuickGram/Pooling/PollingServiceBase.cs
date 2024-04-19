namespace QuickGram.Pooling;

public class PollingServiceBase(ITelegramBotClient botClient, IUpdateHandler handler) : BackgroundService
{
    private readonly ReceiverOptions receiverOptions = new()
    {
        AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
        ThrowPendingUpdates = true,
    };

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //await SendCommands();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await botClient.ReceiveAsync(updateHandler: handler, receiverOptions: receiverOptions, cancellationToken: stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // ignored
            }
            catch (Exception e) 
            {
                try
                {
                    await handler.HandlePollingErrorAsync(botClient, e, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // ignored
                }
            }
        }
    }
}