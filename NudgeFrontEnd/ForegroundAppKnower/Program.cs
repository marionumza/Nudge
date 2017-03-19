using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NudgeToaster;

namespace ForegroundAppKnower
{
    class Program
    {
        public static Timer MyTimer { get; set; }
        private static Process _realProcess;
        static void Main(string[] args)
        {
            MyTimer = new Timer(TimerCallback, null, 0, 1000);
            Console.ReadKey();
        }

        private static void TimerCallback(object state)
        {
            var foregroundProcess = Process.GetProcessById(WinAPIFunctions.GetWindowProcessId(WinAPIFunctions.GetforegroundWindow()));
            if (foregroundProcess.ProcessName == "ApplicationFrameHost")
            {
                foregroundProcess = GetRealProcess(foregroundProcess);
            }
            Console.WriteLine(foregroundProcess.ProcessName);
        }

        private static Process GetRealProcess(Process foregroundProcess)
        {
            WinAPIFunctions.EnumChildWindows(foregroundProcess.MainWindowHandle, ChildWindowCallback, IntPtr.Zero);
            return _realProcess;
        }

        private static bool ChildWindowCallback(IntPtr hwnd, IntPtr lparam)
        {
            var process = Process.GetProcessById(WinAPIFunctions.GetWindowProcessId(hwnd));
            if (process.ProcessName != "ApplicationFrameHost")
            {
                _realProcess = process;
            }
            return true;
        }
    }
}
