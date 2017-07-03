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
using Windows.ApplicationModel.ExtendedExecution;
using Windows.UI.ViewManagement;

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
        private NudgeNotifications nudgeNotifications = new NudgeNotifications();
        private NudgeHarvesterConnection nudgeHarvesterConnection;

        public MainPage()
        {
            this.InitializeComponent();
            api = new API(output);
            nudgeCycle = new NudgeCycle();
            textBoxOutput.Text = "";
            nudgeNotifications.buildNotif();
            nudgeHarvesterConnection = new NudgeHarvesterConnection(this);
            nudgeHarvesterConnection.connectToHarvester();
            ApplicationView.PreferredLaunchViewSize = new Size(704, 885);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">string to be appended</param>
        public async void output(string output)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.textBoxOutput.Text += output + "\n";
                textBoxOutput.Focus(FocusState.Programmatic);
                textBoxOutput.SelectionStart = textBoxOutput.Text.Length - 1;// add some logic if length is 0
                textBoxOutput.SelectionLength = 0;
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await RegisterBackgroundTask();

            return;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            runEngine();
        }

        private  async void RequestExtendedSession()
        {
            var newSession = new ExtendedExecutionSession();
            newSession.Reason = ExtendedExecutionReason.Unspecified;
            newSession.Description = "Raising periodic toasts";
            ExtendedExecutionResult result = await newSession.RequestExtensionAsync();
            output("Extended Session Result: " + result.ToString());
        }

        private bool isTimerRunning;
        TimeSpan engineInterval = new TimeSpan(0, 0, 1);
        private async void runEngine()
        {
            // Start timer that performs call to nudge() after each cycle
            RequestExtendedSession();
            isTimerRunning = true;
            int tick = 0;
            NudgeCycle.getCycleObj().loadCycle();
            while (this.isTimerRunning)
            {
                // 1 tick = 1 second
                tick += 1;
                await Task.Delay(engineInterval);
                NudgeCycle.getCycleObj().loadCycle();
                output("tick " + tick + " / " + NudgeCycle.getCycleObj().getCycle());
                if (tick >= NudgeCycle.getCycleObj().getCycle())
                {
                    // Nudge and reset
                    nudge();
                    isTimerRunning = false;

                }
            }

        }


        private void nudge()
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            nudgeNotifications.ShowNotification();
        }

        private static string BACKGROUND_ENTRY_POINT = typeof(NotificationActionBackgroundTask).FullName;
        private BackgroundTaskRegistration registration;

        private async Task<bool> RegisterBackgroundTask()
        {
            output("Registering Background Task...");
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
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.UserPresent, false));
            builder.SetTrigger(new ToastNotificationActionTrigger());


            // And register the background task
            builder.AddCondition(new SystemCondition(SystemConditionType.UserPresent));
            registration = builder.Register();
            registration.Completed += RegistrationOnCompleted;
            return true;
        }

        private void RegistrationOnCompleted(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        {
            output("Registration of BackgroundTasks Completed");
            if (isTimerRunning == false)
            {
                runEngine();
            }
        }

        private void UnregisterBackgroundTask()
        {
            var task = BackgroundTaskRegistration.AllTasks.Values.FirstOrDefault(i => i.Name.Equals(BACKGROUND_ENTRY_POINT));
            task?.Unregister(true);
            output("Unregistered BackgroundTasks");

        }



    }
}
