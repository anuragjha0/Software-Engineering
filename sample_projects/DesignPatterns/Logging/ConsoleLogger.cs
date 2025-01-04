namespace Logging;

public class ConsoleLogger : ILogger
{
    private readonly LogWriter _logger;

    public ConsoleLogger()
    {
        _logger = new LogWriter(Console.Out);
    }

    public void Log(LogLevel level, string message)
    {
        _logger.Log(level, message);
    }
}
