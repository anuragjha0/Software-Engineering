namespace Logging;

internal class LogWriter
{
    private readonly TextWriter _writer;

    public LogWriter(TextWriter writer)
    {
        _writer = writer;
    }

    public void Log(LogLevel level, string message)
    {
        _writer.WriteLine($"{level}: {message}");
        _writer.Flush();
    }
}