// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="">
//   
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeToaser
{
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;

    using NudgeUtilities;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The short attention span. 15 min
        /// </summary>
        private const int ShortAttentionSpan = 60 * 15;

        /// <summary>
        /// The long attention span. 30 min
        /// </summary>
        private const int LongAttentionSpan = 60 * 30;

        /// <summary>
        /// The timer.
        /// </summary>
        private Timer timer;


        /// <summary>
        /// The current attention span in seconds.
        /// </summary>
        private int currentAttentionSpan;

        /// <summary>
        /// The current threshold. Defaults to short attention span
        /// </summary>
        private int currentThreshold = ShortAttentionSpan;

        /// <summary>
        /// The notification window.
        /// </summary>
        private NotificationWindow notificationWindow;

        /// <summary>
        /// The showing notification.
        /// </summary>
        private bool showingNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.notificationWindow = new NotificationWindow(this);

            this.Engine = new UdpEngine(11111, 22222, this.ReceivedCallback);
            this.Engine.StartUdpServer();
        }

        /// <summary>
        /// Gets the udp engine.
        /// </summary>
        public UdpEngine Engine { get; }

        

        /// <summary>
        /// The yes pressed.
        /// </summary>
        public void YesPressed()
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Engine.SendToClients("YES");
                this.notificationWindow.Visibility = Visibility.Hidden;
                this.Engine.SendToClients("RESUME");
                this.currentAttentionSpan = 0;
                this.currentThreshold = LongAttentionSpan;
                this.showingNotification = false;
            });
        }

        /// <summary>
        /// The no pressed.
        /// </summary>
        public void NoPressed()
        {
            this.Dispatcher.Invoke(() =>
                {
                    this.Engine.SendToClients("NO");
                    this.notificationWindow.Visibility = Visibility.Hidden;
                    this.Engine.SendToClients("RESUME");
                    this.currentAttentionSpan = 0;
                    this.currentThreshold = ShortAttentionSpan;
                    this.showingNotification = false;
                });
        }

        /// <summary>
        /// The received callback.
        /// </summary>
        /// <param name="received">
        /// The received.
        /// </param>
        private void ReceivedCallback(string received)
        {
        }

        /// <summary>
        /// Checks to see if we should nudge user. If attention span passes a threshold, we ask the neural net if we should send a notif to the user.
        /// </summary>
        private void Nudge()
        {

            if (this.currentAttentionSpan > this.currentThreshold)
            {
                if (!this.showingNotification)
                {
                    this.ShowNotification();
                }
            }
        }

        /// <summary>
        /// The show notification.
        /// </summary>
        private void ShowNotification()
        {
            this.showingNotification = true;
            this.Engine.SendToClients("PAUSE");
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.notificationWindow.Show();
                this.notificationWindow.Visibility = Visibility.Visible;
                this.notificationWindow.Topmost = true;
                this.notificationWindow.WindowState = WindowState.Normal;
            });
        }

        /// <summary>
        /// The callback.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void TimerCallback(object state)
        {
            this.currentAttentionSpan++;
            Application.Current.Dispatcher.Invoke(() => { this.TimeLabel.Content = this.currentAttentionSpan.ToString() + "sec"; });
            this.Nudge();
        }

        /// <summary>
        /// The button click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            this.timer = new Timer(this.TimerCallback, null, 0, 1000);
            this.WindowState = WindowState.Minimized;
            this.StartButton.Content = "Started...";
            this.currentAttentionSpan = 0;
        }

        /// <summary>
        /// The window_ closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
