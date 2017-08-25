using System.Windows;

namespace NudgeToaser
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        private MainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationWindow"/> class.
        /// </summary>
        public NotificationWindow(MainWindow window)
        {
            this.mainWindow = window;
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
