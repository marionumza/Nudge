using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace NudgeToaster
{
    class NudgeNotifications
    {
        private ToastContent notif;

        public void buildNotif()
        {
            String time = DateTime.Now.ToString("hh:mm tt");
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
                Scenario = ToastScenario.Reminder,
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

        public void ShowNotification()
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(notif.GetXml()));
        }
    }
}
