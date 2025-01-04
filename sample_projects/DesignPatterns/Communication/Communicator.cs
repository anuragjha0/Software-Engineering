using System.Diagnostics;
using System.Xml;

namespace Communication;

/// <summary>
/// Class that handles sending and receiving messages.
/// </summary>
public class Communicator : ICommunicator
{
    /// <summary>
    /// Creates an instance of the messenger data model.
    /// We listen/send to files instead of dealing with the network.
    /// </summary>
    public Communicator()
    {
        _subscribers = new Dictionary<string, ICommunicationListener>();

        // Setup the receive and send files.
        string receiveFilename = "Received.xml";
        string sendFilename = "Sent.txt";

        try
        {
            string? dir = Environment.GetEnvironmentVariable("temp", EnvironmentVariableTarget.User);
            _receiveFile = Path.Combine(dir, receiveFilename);
            _sendFile = Path.Combine(dir, sendFilename);

            // Setup a watcher on 'receive' file.
            _watcher = new FileSystemWatcher(dir)
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = receiveFilename,
                EnableRaisingEvents = true
            };

            _watcher.Changed += OnFileChanged;
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.Message);
        }
    }

    private static ICommunicator s_communicator; // Singleton instance.
    private static object s_lock;                // Synchronizer.

    static Communicator()
    {
        s_lock = new object();
    }

    /// <summary>
    /// Gets an instance of IMessenger.
    /// </summary>
    /// <returns>An instance of IMessenger</returns>
    public static ICommunicator GetInstance()
    {
        lock (s_lock)
        {
            if (s_communicator == null)
            {
                s_communicator = new Communicator();
            }

            return s_communicator;
        }
    }

    /// <summary>
    /// Lets clients subscribe to notifications from this class.
    /// </summary>
    /// <param name="id">Id of the subscriber</param>
    /// <param name="listener">The subscriber object</param>
    public void Subscribe(string id, ICommunicationListener listener)
    {
        _subscribers.Add(id, listener);
    }

    /// <summary>
    /// Sends a message. In this case, just writes to a 'Send' file.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    public void SendMessage(string message)
    {
        try
        {
            File.WriteAllText(_sendFile, message);
        }
        catch (Exception e)
        {
            Trace.WriteLine(e.Message);
        }
    }

    /// <summary>
    /// Handles the file changed event.
    /// </summary>
    /// <param name="sender">The sender of the notification</param>
    /// <param name="e">The file changed event</param>
    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        // Add some throttling and fault tolerance in case multiple file-changed
        // events are triggered in quick succession and the file is inaccessible.
        string content = string.Empty;
        int attempt = 0;
        bool retry = true;
        do
        {
            ++attempt;
            try
            {
                content = File.ReadAllText(_receiveFile);
                retry = false;
            }
            catch
            {
                Thread.Sleep(attempt * 100);
            }
        } while (retry && (attempt < 3));

        try
        {
            // Pick the first nodes with the details.
            // You may add more error-checking as required.
            XmlDocument? document = new();
            document.LoadXml(content);
            string id = document.GetElementsByTagName("Id")[0].InnerText;
            string message = document.GetElementsByTagName("Message")[0].InnerText;

            // Notify the subscriber.
            _subscribers[id].OnMessageReceived(message);
        }
        catch (Exception exception)
        {
            Trace.WriteLine(exception.Message);
        }
    }

    // The file which we listen to for incoming messages.
    private readonly string _receiveFile;

    // The file where we write the outbound messages to.
    private readonly string _sendFile;

    // Watcher for incoming messages (changes to the 'receive' file).
    private readonly FileSystemWatcher _watcher;

    // The list of subscribers.
    private readonly IDictionary<string, ICommunicationListener> _subscribers;
}
