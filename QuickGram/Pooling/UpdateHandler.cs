using QuickGram.Mediator;

namespace QuickGram.Pooling;

public class UpdateHandler(IServiceProvider serviceProvider) : IUpdateHandler, IDisposable
{
    internal readonly Dictionary<long, (string command, IServiceScope scope)> UsersCommand = [];

    public UpdateHandler(
        Dictionary<long, (string command, IServiceScope scope)> dictionary,
        IServiceProvider serviceProvider) : 
        this(serviceProvider)
    {
        UsersCommand = dictionary;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

        var chat = update.GetChat();
        ArgumentNullException.ThrowIfNull(chat);

        var getCommand = new GetBotCommandNotification { Update = update };
        await publisher.Publish(getCommand, cancellationToken);

        var tuple = ChooseCommand(chat.Id, getCommand.Command);
        if (!tuple.HasValue) return;

        var provider = tuple.Value.scope;
        var command = tuple.Value.command;

        var scopeMediator = provider.ServiceProvider.GetRequiredKeyedService<IMediator>(nameof(CustomTelegramMediator));
        IBotCommandContext updateReceived = new BotCommandContext(update, chat, command);

        try
        {
            await scopeMediator.Publish(updateReceived, cancellationToken);

            ArgumentException.ThrowIfNullOrWhiteSpace(updateReceived.Command);
        }
        catch (Exception e)
        {
            var asyncServiceScope = new AsyncServiceScope(provider);
            await asyncServiceScope.DisposeAsync();

            UsersCommand.Remove(chat.Id);

            throw;
        }

        if (updateReceived.NeedRefresh)
        {
            provider.Dispose();
            UsersCommand.Remove(chat.Id);
        }
    }

    private (IServiceScope scope, string command)? ChooseCommand(long chatId, string botCommand)
    {
        var exist = UsersCommand.TryGetValue(chatId, out var tuple);

        IServiceScope provider;
        var command = botCommand;

        if (string.IsNullOrWhiteSpace(command) && !exist) return null;
        if (!string.IsNullOrWhiteSpace(command) && !exist)
        {
            provider = serviceProvider.CreateScope();
            UsersCommand.Add(chatId, (command, provider));
        }
        else if (string.IsNullOrWhiteSpace(command) && exist)
        {
            provider = tuple.scope;
            command = tuple.command;
        }
        else
        {
            if (tuple.command != command)
            {
                tuple.scope.Dispose();

                provider = serviceProvider.CreateScope();
                UsersCommand[chatId] = (command, provider);
            }
            else
            {
                provider = tuple.scope;
                command = tuple.command;
            }
        }

        return (provider, command);
    }

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing) return;

        foreach (var tuple in UsersCommand)
        {
            tuple.Value.scope.Dispose();
        }
    }
}