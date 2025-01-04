namespace Communication;

public interface ICommunicator
{
    /// <summary>
    /// Lets clients subscribe to notifications from this class.
    /// </summary>
    /// <param name="id">Id of the subscriber</param>
    /// <param name="listener">The subscriber object</param>
    void Subscribe(string id, ICommunicationListener listener);

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="message">Message to be sent</param>
    void SendMessage(string message);
}
