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
        /// The timer.
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The udp engine.
        /// </summary>
        private UdpEngine udpEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.udpEngine = new UdpEngine(11111, 22222);
            udpEngine.StartUdpServer();
        }

        /// <summary>
        /// The callback.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        private void Callback(object state)
        {
            this.udpEngine.SendToClients("TIMER");
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
            this.timer = new Timer(this.Callback, null, 0, 1000);
            this.StartButton.Content = "Started...";
        }

    }
}
