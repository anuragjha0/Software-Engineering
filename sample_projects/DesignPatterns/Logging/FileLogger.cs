namespace Logging;

public class FileLogger : ILogger
{
    private readonly LogWriter _logger;
    private readonly TextWriter _writer;

    public FileLogger()
    {
        _writer = new StreamWriter(File.OpenWrite("Log.txt"));
        _logger = new LogWriter(_writer);
    }

    ~FileLogger()
    {
        _writer.Close();
        _writer.Dispose();
    }

    public void Log(LogLevel level, string message)
    {
        _logger.Log(level, message);
    }
}
