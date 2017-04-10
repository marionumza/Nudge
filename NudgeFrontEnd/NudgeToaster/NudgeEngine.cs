using System;
using System.Threading;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace NudgeToaster
{
    class NudgeEngine
    {
        private Timer engineTimer;
        private ToastContent nudgeToaster;
        private const int cycle = 1000 * 60 * 5;

        private void NudgeEngineTimerCallback(object state)
        {
            nudge();
        }

        public void startEngine()
        {
            engineTimer = new Timer(NudgeEngineTimerCallback, null, cycle, cycle);
        }

        public void nudge()
        {
            buildNotif();
            Show(nudgeToaster);
        }

        private void buildNotif()
        {
            String time = DateTime.Now.ToString("HH:mm tt");
            nudgeToaster = new ToastContent
            {
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

        private static void Show(ToastContent content)
        {
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(content.GetXml()));
        }

    }


}
