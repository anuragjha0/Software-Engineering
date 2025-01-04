using Logging;
using ContentManagement;
using ScreenManagement;
using WhiteBoardManagement;

namespace Executive;

internal class Program
{
    static void Main(string[] _)
    {
        ILogger logger = LoggerFactory.GetInstance(RunningEnvironment.Production);
        ChatManager chatManager = new(logger);
        ScreenManager screenManager = new(logger);
        BoardManager boardManager = new(logger);

        string? message;
        do
        {
            message = Console.ReadLine();
            if (string.IsNullOrEmpty(message))
            {
                // Ignore.
            }
            else if (message.StartsWith(ChatManager.Id))
            {
                chatManager.SendMessage(message);
            }
            else if (message.StartsWith(ScreenManager.Id))
            {
                screenManager.SendMessage(message);
            } else if (message.StartsWith(BoardManager.Id))
            {
                boardManager.SendMessage(message);
            }
        } while (message != "quit");
    }
}
