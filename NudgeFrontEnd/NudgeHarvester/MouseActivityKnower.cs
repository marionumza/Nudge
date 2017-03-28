// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MouseActivityKnower.cs" company="Sammy Guergachi">
//   Sammy Guergachi 2017
// </copyright>
// <summary>
//   The mouse activity knower.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeHarvester
{
    using System;
    using System.Net.Mime;
    using System.Timers;

    using Gma.System.MouseKeyHook;

    using static System.Windows.Forms.Form;

    /// <summary>
    /// The mouse activity knower.
    /// </summary>
    public class MouseActivityKnower
    {

        /// <summary>
        /// The activity timer.
        /// </summary>
        private readonly Timer inactivityTimer;

        /// <summary>
        /// The elapsed inactivity.
        /// </summary>
        private int elapsedInactivity = 0;

        /// <summary>
        /// The mouse global hook.
        /// </summary>
        private IKeyboardMouseEvents mouseGlobalHook;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseActivityKnower"/> class.
        /// </summary>
        public MouseActivityKnower()
        {
            this.inactivityTimer = new Timer(HarvesterProgram.Cycle - 100); // minus 100ms to check before the loop timer checks
            this.inactivityTimer.Elapsed += this.InactiveTimerCallback;
            this.SubscribeToMouseEvents();

            if (ActiveForm != null)
            {
                ActiveForm.FormClosing += this.UnsubscribeFromMouseEvents;
            }

            this.inactivityTimer.Start();
        }

        /// <summary>
        /// The is mouse active.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public int GetInactiveMouseElapsed()
        {
            return this.elapsedInactivity;
        }

        /// <summary>
        /// The reset timer.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void InactiveTimerCallback(object state, ElapsedEventArgs e)
        {
            this.elapsedInactivity += e.SignalTime.Millisecond;
        }

        /// <summary>
        /// The subscribe.
        /// </summary>
        private void SubscribeToMouseEvents()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            this.mouseGlobalHook = Hook.GlobalEvents();

            this.mouseGlobalHook.MouseMoveExt += this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseUpExt += this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseUpExt += this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseWheelExt += this.GlobalHookMouseActivityExt;
        }

        /// <summary>
        /// The unsubscribe.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void UnsubscribeFromMouseEvents(object sender, EventArgs e)
        {
            this.mouseGlobalHook.MouseMoveExt -= this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseUpExt -= this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseUpExt -= this.GlobalHookMouseActivityExt;
            this.mouseGlobalHook.MouseWheelExt -= this.GlobalHookMouseActivityExt;

            this.mouseGlobalHook.Dispose();
        }


        /// <summary>
        /// The global hook mouse move ext.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GlobalHookMouseActivityExt(object sender, MouseEventExtArgs e)
        {
            this.elapsedInactivity = 0;
            this.inactivityTimer.Stop();
            this.inactivityTimer.Start();
        }


    }
}
