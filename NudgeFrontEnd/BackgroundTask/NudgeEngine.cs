using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.ApplicationModel.Resources;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using BackgroundTasks;

namespace BackgroundTasks
{
    public sealed class NudgeEngine
    {

        static int cycle = 1000 * 60 * 15;



        public int getCycle()
        {
            return cycle;
        }

        public static void setCycle(int i)
        {
            cycle = i;
        }
    }


}
