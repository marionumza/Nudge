// --------------------------------------------------------------------------------------------------------------------
// <copyright file="KeyboardActivityKnower.cs" company="Sammy Guergachi">
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
    using System.Windows.Forms;

    using Gma.System.MouseKeyHook;

    using static System.Windows.Forms.Form;

    using Timer = System.Timers.Timer;

    /// <summary>
    /// The mouse activity knower.
    /// </summary>
    public class KeyboardActivityKnower
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
        private IKeyboardMouseEvents keyboardGlobalHook;

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseActivityKnower"/> class.
        /// </summary>
        public KeyboardActivityKnower()
        {
            this.inactivityTimer = new Timer(HarvesterProgram.Cycle - 100); // minus 100ms to check before the loop timer checks
            this.inactivityTimer.Elapsed += this.InactiveTimerCallback;
            this.SubscribeToKeyboardEvents();

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
        public int GetInactiveKeyboardElapsed()
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
        private void SubscribeToKeyboardEvents()
        {
            // Note: for the application hook, use the Hook.AppEvents() instead
            this.keyboardGlobalHook = Hook.GlobalEvents();

            this.keyboardGlobalHook.KeyDown += this.GlobalHookKeyboardActivityExt;
            this.keyboardGlobalHook.KeyUp += this.GlobalHookKeyboardActivityExt;
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
            this.keyboardGlobalHook.KeyDown -= this.GlobalHookKeyboardActivityExt;
            this.keyboardGlobalHook.KeyUp -= this.GlobalHookKeyboardActivityExt;

            this.keyboardGlobalHook.Dispose();
        }


        /// <summary>
        /// The global hook keyboard.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void GlobalHookKeyboardActivityExt(object sender, KeyEventArgs e)
        {
            this.elapsedInactivity = 0;
            this.inactivityTimer.Stop();
            this.inactivityTimer.Start();
        }


    }
}
