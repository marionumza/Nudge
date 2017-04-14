using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using NotificationsExtensions.Toasts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NudgeToaster
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private API api;
        private NudgeEngine engine;

        public MainPage()
        {
            this.InitializeComponent();
            api = new API(output);
            engine = new NudgeEngine(output);
            textBoxOutput.Text = "";

        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        public void output(string output)
        {
            textBoxOutput.Text = textBoxOutput.Text + output + Environment.NewLine;
            Debug.WriteLine(output);
        }

        private void auth_Click(object sender, RoutedEventArgs e)
        {
            api.authGoogleCloud();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            api.ProcessAuth(e);
        }

        private void PredictButton_Click(object sender, RoutedEventArgs e)
        {
            api.TestPredict();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            engine.startEngine();
        }
        
        private  void Page_Loaded(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}
