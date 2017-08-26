

namespace NudgeHarvester
{
    using NudgeUtilities;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// The harvester program.
    /// </summary>
    internal class HarvesterProgram
    {
        /// <summary>
        /// The cycle.
        /// </summary>
        public const int Cycle = 1000;

        /// <summary>
        /// The delay.
        /// </summary>
        public const int Delay = 0;

        /// <summary>
        /// The my foreground app knower.
        /// </summary>
        private ForegroundAppKnower myForegroundAppKnower = new ForegroundAppKnower();

        /// <summary>
        /// The my attention span knower.
        /// </summary>
        private AttentionSpanKnower myAttentionSpanKnower = new AttentionSpanKnower();

        /// <summary>
        /// The my mouse activity knower.
        /// </summary>
        private MouseActivityKnower myMouseActivityKnower = new MouseActivityKnower();

        /// <summary>
        /// The my keyboard activity knower.
        /// </summary>
        private KeyboardActivityKnower myKeyboardActivityKnower = new KeyboardActivityKnower();

        /// <summary>
        /// The udp engine.
        /// </summary>
        private UdpEngine udpEngine;


        /// <summary>
        /// Initializes a new instance of the <see cref="HarvesterProgram"/> class.
        /// </summary>
        /// <param name="nudgeHarvesterForm">The Form</param>
        public HarvesterProgram(NudgeHarvesterForm nudgeHarvesterForm)
        {
            this.NudgeHarvesterForm = nudgeHarvesterForm ?? throw new ArgumentNullException(nameof(nudgeHarvesterForm));

            this.udpEngine = new UdpEngine(22222, 11111);
            this.udpEngine.StartUdpServer();

            this.LoopTimer = new Timer { Interval = Cycle };
            this.LoopTimer.Tick += this.TimerCallback;
            this.LoopTimer.Start();



            Application.ApplicationExit += (sender, args) =>
            {
                this.LoopTimer.Dispose();
            };
        }


        /// <summary>
        /// Gets the nudge harvester form.
        /// </summary>
        public NudgeHarvesterForm NudgeHarvesterForm { get; }

        /// <summary>
        /// Gets or sets the loop timer.
        /// </summary>
        public Timer LoopTimer { get; set; }


        /// <summary>
        /// The timer callback.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private async void TimerCallback(object state, EventArgs e)
        {
            this.myAttentionSpanKnower.Increment(Cycle);
            this.NudgeHarvesterForm.OutputText("Current Foreground App: " + this.myForegroundAppKnower.GetForegroundApp());
            this.NudgeHarvesterForm.OutputText("Current Foreground App Int: " + this.myForegroundAppKnower.GetForegroundApp().GetHashCode());
            this.NudgeHarvesterForm.OutputText("Mouse Inactive For: " + this.myMouseActivityKnower.GetInactiveMouseElapsed() + "ms");
            this.NudgeHarvesterForm.OutputText("Keyboard Inactive For: " + this.myKeyboardActivityKnower.GetInactiveKeyboardElapsed() + "ms");
            this.NudgeHarvesterForm.OutputText("Current Attention Span: " + this.myAttentionSpanKnower.GetAttentionSpan() + "ms");
            this.NudgeHarvesterForm.OutputText(string.Empty);
            udpEngine.SendToClients("Ping!");
        }



    }

}
