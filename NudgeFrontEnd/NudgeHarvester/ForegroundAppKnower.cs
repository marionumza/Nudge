// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ForegroundAppKnower.cs" company="Sammy Guergachi">
// Sammy Guergachi 2017
// </copyright>
// <summary>
//   The foreground app knower.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeHarvester
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// The foreground app knower.
    /// </summary>
    public class ForegroundAppKnower
    {
        /// <summary>
        /// The real process.
        /// </summary>
        private Process realProcess;

        /// <summary>
        /// The get foreground app.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetForegroundApp()
        {
            var foregroundProcess = Process.GetProcessById(WindowsApiFunctions.GetWindowProcessId(WindowsApiFunctions.GetforegroundWindow()));
            if (foregroundProcess.ProcessName == "ApplicationFrameHost")
            {
                foregroundProcess = this.GetRealProcess(foregroundProcess);
            }
            if (foregroundProcess == null)
            {
                return this.GetForegroundApp();
            }
            else
            {
                return foregroundProcess.ProcessName;
            }

        }

        /// <summary>
        /// The get real process.
        /// </summary>
        /// <param name="foregroundProcess">
        /// The foreground process.
        /// </param>
        /// <returns>
        /// The <see cref="Process"/>.
        /// </returns>
        private Process GetRealProcess(Process foregroundProcess)
        {
            WindowsApiFunctions.EnumChildWindows(foregroundProcess.MainWindowHandle, this.ChildWindowCallback, IntPtr.Zero);
            return this.realProcess;
        }

        /// <summary>
        /// The child window callback.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="lparam">
        /// The lparam.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool ChildWindowCallback(IntPtr hwnd, IntPtr lparam)
        {
            var process = Process.GetProcessById(WindowsApiFunctions.GetWindowProcessId(hwnd));
            if (process.ProcessName != "ApplicationFrameHost")
            {
                this.realProcess = process;
            }
            return true;
        }
    }

    /// <summary>
    /// The win api functions.
    /// </summary>
    internal class WindowsApiFunctions
    {
        // Used to get Handle for Foreground Window
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetForegroundWindow();

        // Used to get ID of any Window
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
        public delegate bool WindowEnumProc(IntPtr hwnd, IntPtr lparam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc callback, IntPtr lParam);

        /// <summary>
        /// The get window process id.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public static int GetWindowProcessId(IntPtr hwnd)
        {
            int pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        /// <summary>
        /// The getforeground window.
        /// </summary>
        /// <returns>
        /// The <see cref="IntPtr"/>.
        /// </returns>
        public static IntPtr GetforegroundWindow()
        {
            return GetForegroundWindow();
        }
    }
}
