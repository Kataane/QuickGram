namespace QuickGram.BotCommands;

public class TestCommandHandler : TelegramCommandHandler
{
    public override string Description { get; }
    public override string Command => "/hi";

    private readonly ITelegramBotClient botClient;

    public TestCommandHandler(ITelegramBotClient botClient)
    {
        this.botClient = botClient;
    }

    protected override async Task Core(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(notification.Chat, "Hi", cancellationToken: cancellationToken);
    }
}