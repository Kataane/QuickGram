using QuickGram.Behaviors;

namespace QuickGram.BotBehaviors;

public class BotCommandContext(Update update, Chat chat, string command) : IBotCommandContext, IRequestPerformance, IRequestLogger
{
    public Update Update { get; } = update;
    public Chat Chat { get; } = chat;
    public string Command { get; } = command;

    public Message? Message { get; private set; }

    public IReadOnlyCollection<string>? Args { get; private set; }

    public bool NeedRefresh { get; private set; }

    public void AddMessage(Message? message)
    {
        Message = message;
    }

    public void AddArgs(IEnumerable<string> args)
    {
        Args = args.ToList();
    }

    public void SetNeedRefresh()
    {
        NeedRefresh = true;
    }
}