namespace Logging;

public static class LoggerFactory
{
    public static ILogger GetInstance(RunningEnvironment type)
    {
        ILogger logger = type switch
        {
            // For development, use console logging.
            // For production, use file logging.

            RunningEnvironment.Development => new ConsoleLogger(),
            RunningEnvironment.Production => new FileLogger(),
            _ => throw new ArgumentException("Unsupported logger type"),
        };

        return logger;
    }
}
