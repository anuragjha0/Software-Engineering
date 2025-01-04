using System.Windows;
using System.Windows.Controls;

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Page messengerPage = new MessengerPage();
            MainFrame.Navigate(messengerPage);
        }
    }
}
