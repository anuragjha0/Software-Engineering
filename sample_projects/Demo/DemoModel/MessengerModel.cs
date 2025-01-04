using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace DemoModel
{
    /// <summary>
    /// Data model for the messenger application.
    /// </summary>
    public class MessengerModel
    {
        /// <summary>
        /// Creates an instance of the messenger data model.
        /// We listen/send to files instead of dealing with the network.
        /// </summary>
        /// <param name="listener">The subscriber</param>
        public MessengerModel(IMessageListener listener)
        {
            _client = listener;

            // Setup the receive and send files.
            string receiveFilename = "Received.xml";
            string sendFilename = "Sent.txt";
            string dir = Environment.GetEnvironmentVariable("temp", EnvironmentVariableTarget.User);
            _receiveFile = Path.Combine(dir, receiveFilename);
            _sendFile = Path.Combine(dir, sendFilename);

            // Notify the client, asynchronously.
            _ = Task.Run(this.NotifyClient);

            try
            {
                // Setup a watcher on 'receive' file.
                _watcher = new FileSystemWatcher(dir)
                {
                    NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size,
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

        /// <summary>
        /// Sends a message. In this case, just writes to a 'Send' file.
        /// </summary>
        /// <param name="message">Message to be sent.</param>
        public void SendMessage(string message)
        {
            // Run asynchronously.
            _ = Task.Run(() =>
                    {
                        try
                        {
                            File.WriteAllText(_sendFile, message);
                        }
                        catch (Exception e)
                        {
                            Trace.WriteLine(e.Message);
                        }
                    });
        }

        /// <summary>
        /// Handles the file changed event.
        /// </summary>
        /// <param name="sender">The sender of the notification</param>
        /// <param name="e">The file changed event</param>
        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            this.NotifyClient();
        }

        /// <summary>
        /// Notifies the subscriber.
        /// </summary>
        private void NotifyClient()
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
                XmlDocument document = new XmlDocument();
                document.LoadXml(content);
                string caption = document.GetElementsByTagName("Caption")[0].InnerText;
                string imagePath = document.GetElementsByTagName("Image")[0].InnerText;

                // Notify the subscriber.
                if (_client != null)
                {
                    _client.OnMessageReceived(imagePath, caption);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }

        // The file which we listen to for incoming messages.
        private readonly string _receiveFile;

        // The file where we write the outbound messages to.
        private readonly string _sendFile;

        // Watcher for incoming messages (changes to the 'receive' file).
        private readonly FileSystemWatcher _watcher;

        // The subscriber.
        private IMessageListener _client;
    }
}
