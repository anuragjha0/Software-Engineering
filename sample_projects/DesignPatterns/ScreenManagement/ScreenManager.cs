using Communication;
using Logging;

namespace ScreenManagement;

public class ScreenManager : ICommunicationListener
{
    public const string Id = "screen";

    private readonly ICommunicator _communicator;
    private readonly ILogger _logger;

    public ScreenManager(ILogger logger)
    {
        _communicator = Communicator.GetInstance();
        _communicator.Subscribe(Id, this);
        _logger = logger;
    }

    public void SendMessage(string message)
    {
        string info = $"{Id}: {message}";
        _logger.Log(LogLevel.Information, $"Sending: {info}");
        _communicator.SendMessage(info);
    }

    public void OnMessageReceived(string message)
    {
        string info = $"{Id}: {message}";
        _logger.Log(LogLevel.Information, $"Received: {info}");
        Console.WriteLine(info);
    }
}

