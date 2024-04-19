namespace QuickGram.Behaviors;

public class RequestPerformanceBehaviour<TRequest, TResponse>(
    TimeProvider timeProvider,
    ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequestPerformance
{
    private readonly ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly TimeProvider timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var start = timeProvider.GetTimestamp();

        var response = await next();

        var diff = timeProvider.GetElapsedTime(start);

        if (diff.TotalMilliseconds < 500) return response;

        var name = typeof(TRequest).Name;

        logger.LongRunningRequest(name, diff, request);

        return response;
    }
}

internal static partial class LoggerExtensions
{
    [LoggerMessage(EventId = 1, EventName = nameof(LongRunningRequest), Level = LogLevel.Warning, Message = "Long Running Request: {Name} ({Elapsed} milliseconds) {Request}")]
    public static partial void LongRunningRequest(this ILogger logger, string name, TimeSpan elapsed, object request);
}

public interface IRequestPerformance;