namespace QuickGram;

public static class UpdateExtensions
{
    /// <summary>
    /// Returns chat for updates with chat, null otherwise. Exception in case of new update type.   
    /// </summary>
    /// <param name="update"></param>
    /// <returns>Chat or null (for updates that doesn't have chat</returns>
    /// <exception cref="ArgumentOutOfRangeException">Throws exception if there is some new update.Type</exception>
    public static Chat? GetChat(this Update update) => update.Type switch
    {
        UpdateType.Message => update.Message!.Chat,
        UpdateType.CallbackQuery when update.CallbackQuery!.Message is not null => update.CallbackQuery!.Message.Chat,
        UpdateType.Unknown => null,
        UpdateType.InlineQuery => null,
        UpdateType.ChosenInlineResult => null,
        UpdateType.EditedMessage => null,
        UpdateType.ChannelPost => null,
        UpdateType.EditedChannelPost => null,
        UpdateType.ShippingQuery => null,
        UpdateType.PreCheckoutQuery => null,
        UpdateType.Poll => null,
        UpdateType.PollAnswer => null,
        UpdateType.MyChatMember => null,
        UpdateType.ChatMember => null,
        UpdateType.ChatJoinRequest => null,
        _ => throw new ArgumentOutOfRangeException(nameof(update), update.Type, null)
    };
}