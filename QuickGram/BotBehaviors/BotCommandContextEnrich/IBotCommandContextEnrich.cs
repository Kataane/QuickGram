namespace QuickGram.BotBehaviors.BotCommandContextEnrich;

public interface IBotCommandContextEnrich
{
    protected internal bool CanHandle(IBotCommandContext notification);

    protected internal Message GetMessage(IBotCommandContext notification);

    protected internal IEnumerable<string> GetArgs(IBotCommandContext notification);
}