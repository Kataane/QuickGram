namespace QuickGram.BotBehaviors;

public interface IBotCommandContext : INotification
{
    public Update Update { get; }

    public Chat Chat { get; }

    public string Command { get; }

    public Message? Message { get; }

    public IReadOnlyCollection<string>? Args { get; }

    public bool NeedRefresh { get;  }

    public void AddMessage(Message? message);

    public void AddArgs(IEnumerable<string> args);

    public void SetNeedRefresh();
}