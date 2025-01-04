namespace Communication;

/// <summary>
/// Interface to notify clients when a message has been received.
/// </summary>
public interface ICommunicationListener
{
    /// <summary>
    /// Handles the reception of a message.
    /// </summary>
    /// <param name="message">Received message.</param>
    void OnMessageReceived(string message);
}
