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
    }
}
