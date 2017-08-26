using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NudgeToaser
{
    using System.Threading;

    using NudgeUtilities;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The short attention span. 15 min
        /// </summary>
        private const int ShortAttentionSpan = 15000;

        /// <summary>
        /// The long attention span. 30 min
        /// </summary>
        private const int LongAttentionSpan = 30000;

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
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            notificationWindow = new NotificationWindow(this.Window);

            this.Engine = new UdpEngine(11111, 22222, this.ReceivedCallback);
            this.Engine.StartUdpServer();
        }

        /// <summary>
        /// Gets the udp engine.
        /// </summary>
        public UdpEngine Engine { get; }

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
            if (this.currentAttentionSpan > currentThreshold)
            {
                this.ShowNotification();
            }
        }

        /// <summary>
        /// The show notification.
        /// </summary>
        private void ShowNotification()
        {
            this.Engine.SendToClients("PAUSE");
            this.notificationWindow.Visibility = Visibility.Visible;
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
            this.StartButton.Content = "Started...";
        }

    }
}
