namespace QuickGram.BotCommands;

public class ExceptionCommandHandler : TelegramCommandHandler
{
    public override string Description { get; }

    public override string Command => "/exception";

    protected override Task Core(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException("Кажется что-то сломалось!");
    }
}