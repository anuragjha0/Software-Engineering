using System.Windows;
using System.Windows.Controls;
using DemoViewModel;

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for MessagingPage.xaml
    /// </summary>
    public partial class MessengerPage : Page
    {
        public MessengerPage()
        {
            InitializeComponent();

            MessengerViewModel viewModel = new MessengerViewModel();
            this.DataContext = viewModel;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.SendTextBox.Text))
            {
                string text = this.SendTextBox.Text;
                this.SendTextBox.Text = string.Empty;

                MessengerViewModel viewModel = this.DataContext as MessengerViewModel;
                viewModel.OutboundMessage = text;
            }
        }
    }
}
