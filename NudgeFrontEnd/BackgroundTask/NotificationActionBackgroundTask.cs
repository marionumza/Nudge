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
                NudgeCycle.getCycleObj().setTaskInstance(taskInstance);

                if (arguments.Contains("Yes"))
                {
                    Debug.WriteLine("Yes Cycle: " + 60 * 30);
                    NudgeCycle.getCycleObj().setCycle(60 * 30);

                }
                else
                {
                    Debug.WriteLine("No Cycle: " + 60 * 15);
                    NudgeCycle.getCycleObj().setCycle(60 * 15);
                }
            }
        }
    }


}
