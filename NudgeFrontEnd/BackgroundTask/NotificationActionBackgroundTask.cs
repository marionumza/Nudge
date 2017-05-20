using System.Diagnostics;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
namespace BackgroundTasks
{
    public sealed class NotificationActionBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;

            if (details != null)
            {
                string arguments = details.Argument;
                var userInput = details.UserInput;

                if (arguments.Contains("Yes"))
                {
                    Debug.WriteLine("Yes");
                    //                    NudgeCycle.setCycle(1000 * 60 * 30);
                    NudgeCycle.getCycleObj().setCycle(60 * 30);

                }
                else
                {
                    //                    NudgeCycle.setCycle(1000 * 60 * 5);
                    NudgeCycle.getCycleObj().setCycle(60 * 5);
                    Debug.WriteLine("No");
                }
            }
        }
    }


}
