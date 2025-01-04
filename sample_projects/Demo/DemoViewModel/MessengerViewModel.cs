using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DemoModel;

namespace DemoViewModel
{
    /// <summary>
    /// ViewModel for the Messenger page.
    /// </summary>
    public class MessengerViewModel :
        INotifyPropertyChanged, // Notifies clients that a property value has changed.
        IMessageListener        // Notifies clients that has a message has been received.
    {
        /// <summary>
        /// Creates an instance of the Messenger ViewModel.
        /// </summary>
        public MessengerViewModel()
        {
            _model = new MessengerModel(this);
        }

        /// <summary>
        /// The message to be sent.
        /// </summary>
        public string OutboundMessage
        {
            set => _model.SendMessage(value);
        }

        /// <summary>
        /// The received image.
        /// </summary>
        public BitmapImage ReceivedImage
        {
            get; private set;
        }

        /// <summary>
        /// The received caption.
        /// </summary>
        public string ReceivedCaption
        {
            get; private set;
        }

        /// <summary>
        /// Handles an incoming message.
        /// </summary>
        /// <param name="imagePath">The path to the image.</param>
        /// <param name="caption">The caption for the image.</param>
        public void OnMessageReceived(string imagePath, string caption)
        {
            // Execute the call on the application's main thread.
            //
            // Also note that we may execute the call asynchronously as the calling
            // thread is not dependent on the callee thread finishing this method call.
            // Hence we may call the dispatcher's BeginInvoke method which kicks off
            // execution asynchronously as opposed to Invoke which does it synchronously.

            _ = this.ApplicationMainThreadDispatcher.BeginInvoke(
                        DispatcherPriority.Normal,
                        new Action<string, string>((path, text) =>
                        {
                            lock (this)
                            {
                                // Note that Bitmap cannot be automatically marshaled to the main thread
                                // if it were created on the worker thread. Hence the data model just passes
                                // the path to the image, and the main thread creates an image from it.

                                BitmapImage image = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
                                this.ReceivedImage = image;
                                this.ReceivedCaption = text;

                                this.OnPropertyChanged("ReceivedImage");
                                this.OnPropertyChanged("ReceivedCaption");
                            }
                        }),
                        imagePath,
                        caption);
        }

        /// <summary>
        /// Property changed event raised when a property is changed on a component.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handles the property changed event raised on a component.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Gets the dispatcher to the main thread. In case it is not available
        /// (such as during unit testing) the dispatcher associated with the
        /// current thread is returned.
        /// </summary>
        private Dispatcher ApplicationMainThreadDispatcher =>
            (Application.Current?.Dispatcher != null) ?
                    Application.Current.Dispatcher :
                    Dispatcher.CurrentDispatcher;

        /// <summary>
        /// Underlying data model.
        /// </summary>
        private MessengerModel _model;
    }
}
