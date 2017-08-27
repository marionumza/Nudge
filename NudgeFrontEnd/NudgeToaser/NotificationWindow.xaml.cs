using System.Windows;

namespace NudgeToaser
{
    /// <summary>
    /// Interaction logic for NotificationWindow.xaml
    /// </summary>
    public partial class NotificationWindow : Window
    {
        /// <summary>
        /// The main window.
        /// </summary>
        private MainWindow mainWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationWindow"/> class.
        /// </summary>
        /// <param name="window">
        /// The window.
        /// </param>
        public NotificationWindow(MainWindow window)
        {
            this.mainWindow = window;

            this.InitializeComponent();
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        /// <summary>
        /// The button_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            this.mainWindow.YesPressed();
        }

        /// <summary>
        /// The no button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            this.mainWindow.NoPressed();
        }
    }
}
