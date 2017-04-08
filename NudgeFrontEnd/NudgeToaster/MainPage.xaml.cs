using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
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

        public MainPage()
        {
            this.InitializeComponent();
            api = new API(this.textBoxOutput);
        }

        private void Show(ToastContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

        private void notif_Click(object sender, RoutedEventArgs e)
        {
            api.TestAPI();


            String time = DateTime.Now.ToString("HH:mm tt");
            Show(new ToastContent()
            {
                Visual = new ToastVisual()
                {

                    TitleText = new ToastText() { Text = "It's currently " + time },
                    BodyTextLine1 = new ToastText() { Text = "Is this really what you want to be doing right now? " }
                },

                Launch = "394815",

                Scenario = ToastScenario.Default,

                Actions = new ToastActionsCustom()
                {
                    Buttons =
                    {
                        new ToastButton("Yes", "Yes" ),
                        new ToastButton("No", "No")
                    }
                }
            });
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

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
