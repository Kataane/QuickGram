using QuickGram.Behaviors;
using QuickGram.BotBehaviors.BotCommandContextEnrich;
using QuickGram.BotCommands;
using QuickGram.Mediator;
using QuickGram.Pooling;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        var token = sp.GetRequiredService<IConfiguration>().GetSection("Configuration:BotToken").Value;
        ArgumentException.ThrowIfNullOrEmpty(token);

        TelegramBotClientOptions options = new(token);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddSingleton<IUpdateHandler, UpdateHandler>();
builder.Services.AddHostedService<PollingServiceBase>();

builder.Services.AddMediatR(static cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddSingleton<IBotCommandContextEnrich, MessageUpdateReceiver>();
builder.Services.AddSingleton<IBotCommandContextEnrich, CallbackQueryUpdateReceiver>();

builder.Services.AddKeyedScoped<IMediator, CustomTelegramMediator>(nameof(CustomTelegramMediator));

builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(BotCommandCounterBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggerBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(BotCommandExceptionBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PrepareBotCommandContextBehavior<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AddUserBehaviour<,>));
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

builder.Services.AddScoped(typeof(INotificationHandler<BotCommandContext>), typeof(TestCommandHandler));
builder.Services.AddScoped(typeof(INotificationHandler<BotCommandContext>), typeof(ExceptionCommandHandler));
builder.Services.AddScoped(typeof(INotificationHandler<BotCommandContext>), typeof(SignupCommandHandler));

var app = builder.Build();

app.Run();