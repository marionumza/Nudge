using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
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
using BackgroundTasks;
using NotificationsExtensions;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NudgeToaster
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private API api;
        private NudgeCycle nudgeCycle;

        public MainPage()
        {
            this.InitializeComponent();
            api = new API(output);
            nudgeCycle = new NudgeCycle();
            textBoxOutput.Text = "";

        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        public async void output(string output)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.textBoxOutput.Text = output;
            });
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


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            return;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            runEngine();
        }

        Timer engineTimer;
        private void runEngine()
        {
            // Start timer that performs call to nudge() after each cycle
            int tick = 0;
            NudgeCycle.getCycleObj().loadCycle();
            engineTimer = new Timer(delegate
            {
                tick += 1;
                Debug.WriteLine("tick " + tick + " / " + NudgeCycle.getCycleObj().getCycle());
                if (tick >= NudgeCycle.getCycleObj().getCycle())
                {
                    nudge();
                    engineTimer.Dispose();
                    runEngine();
                }
            }, null, 1, 1000);
        }


        public async void nudge()
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();
            // Register background task
            if (!await RegisterBackgroundTask())
            {
                await new MessageDialog("ERROR: Couldn't register background task.").ShowAsync();
                return;
            }
            buildNotif();
            Show(notif);
        }

        private static void Show(ToastContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

        private static string BACKGROUND_ENTRY_POINT = typeof(NotificationActionBackgroundTask).FullName;
        private BackgroundTaskRegistration registration;

        public async Task<bool> RegisterBackgroundTask()
        {
            // Unregister any previous exising background task
            UnregisterBackgroundTask();

            // Request access
            BackgroundAccessStatus status = await BackgroundExecutionManager.RequestAccessAsync();

            // If denied
            if (status != BackgroundAccessStatus.AlwaysAllowed && status != BackgroundAccessStatus.AllowedSubjectToSystemPolicy)
                return false;

            // Construct the background task
            BackgroundTaskBuilder builder = new BackgroundTaskBuilder()
            {
                Name = BACKGROUND_ENTRY_POINT,
                TaskEntryPoint = BACKGROUND_ENTRY_POINT
            };

            // Set trigger for Toast History Changed
            builder.SetTrigger(new ToastNotificationActionTrigger());


            // And register the background task
            registration = builder.Register();
            registration.Progress += OnProgress;
            registration.Completed += RegistrationOnCompleted;
            return true;
        }

        private void RegistrationOnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            output("Completed");
        }

        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            var progress = "Progress: " + args.Progress + "%";
            output(progress);
        }

        private static void UnregisterBackgroundTask()
        {
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(i => i.Name.Equals(BACKGROUND_ENTRY_POINT));
            task?.Unregister(true);
        }

        private ToastContent notif;

        private void buildNotif()
        {

            String time = DateTime.Now.ToString("HH:mm tt");
            notif = new ToastContent
            {
                ActivationType = ToastActivationType.Background,
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "It's currently " + time,
                                HintStyle = AdaptiveTextStyle.Header
                            },
                            new AdaptiveText()
                            {
                                Text = "Is this really what you want to be doing right now? ",
                                HintStyle = AdaptiveTextStyle.Body,
                                HintWrap = true
                            },
                            new AdaptiveImage()
                            {
                                Source = "Assets/Nudge.png"
                            }

                        }
                    }
                },
                Launch = "394815",
                Scenario = ToastScenario.Default,
                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButton("Yes", "Yes" )
                        {
                            ActivationType = ToastActivationType.Background
                        },
                        new ToastButton("No", "No")
                        {
                            ActivationType = ToastActivationType.Background
                        }
                    }
                }
            };
        }
    }
}
