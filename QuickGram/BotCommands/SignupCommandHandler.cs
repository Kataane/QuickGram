namespace QuickGram.BotCommands;

public class SignupCommandHandler(ITelegramBotClient botClient) : TelegramCommandHandler
{
    public const string CommandValue = "/signup";
    public const string DescriptionValue = "Зарегистрироваться";

    public override string Description => DescriptionValue;
    public override string Command => CommandValue;

    private readonly ITelegramBotClient botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));

    private string firstname;
    private string surname;
    private string age;

    protected override Task Core(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(notification.Message?.Text);

        switch (Calls)
        {
            case 2:
                firstname = notification.Message.Text;
                break;
            case 3:
                surname = notification.Message.Text;
                break;
            case 4:
                age = notification.Message.Text;
                break;
        }

        return Calls switch
        {
            1 => EnterFirstname(notification, cancellationToken),
            2 => EnterSurname(notification, cancellationToken),
            3 => EnterAge(notification, cancellationToken),
            4 => AddNewUser(notification, cancellationToken),
            _ => Task.CompletedTask,
        };
    }

    private async Task EnterFirstname(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        const string text = "Пожалуйтса, введите Ваше имя";
        await botClient.SendTextMessageAsync(chatId: notification.Chat, text: text, cancellationToken: cancellationToken);
    }

    private async Task EnterSurname(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        const string text = "Пожалуйтса, введите Вашу фамилию";
        await botClient.SendTextMessageAsync(chatId: notification.Chat, text: text, cancellationToken: cancellationToken);
    }

    private async Task EnterAge(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        const string text = "Пожалуйтса, введите Ваш возраст";
        await botClient.SendTextMessageAsync(chatId: notification.Chat, text: text, cancellationToken: cancellationToken);
    }

    private async Task AddNewUser(IBotCommandContext notification, CancellationToken cancellationToken)
    {
        var text = "Регистрирую.";
        await botClient.SendTextMessageAsync(chatId: notification.Chat, text: text, cancellationToken: cancellationToken);

        text = $"Добро пожаловать! {firstname} {surname}";
        await botClient.SendTextMessageAsync(chatId: notification.Chat, text: text, cancellationToken: cancellationToken);
    }

    public override ValueTask<bool> IsLastStage()
    {
        return ValueTask.FromResult(Calls >= 4);
    }
}