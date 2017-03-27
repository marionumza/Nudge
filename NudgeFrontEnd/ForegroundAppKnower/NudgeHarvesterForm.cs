using System.Windows.Forms;

namespace NudgeHarvester
{
    using System;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class NudgeHarvesterForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeHarvesterForm"/> class.
        /// </summary>
        public NudgeHarvesterForm()
        {
            this.InitializeComponent();
            HarvesterProgram havester = new HarvesterProgram(this);

        }

        /// <summary>
        /// The output text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public void OutputText(string text)
        {
            this.outputBox.Items.Add(text);
            int visibleItems = this.outputBox.ClientSize.Height / this.outputBox.ItemHeight;
            this.outputBox.TopIndex = Math.Max(this.outputBox.Items.Count - visibleItems + 1, 0);
        }

    }

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
        /// Gets the nudge harvester form.
        /// </summary>
        public NudgeHarvesterForm NudgeHarvesterForm { get; }

        /// <summary>
        /// Gets or sets the loop timer.
        /// </summary>
        public Timer LoopTimer { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HarvesterProgram"/> class.
        /// </summary>
        /// <param name="nudgeHarvesterForm"></param>
        public HarvesterProgram(NudgeHarvesterForm nudgeHarvesterForm)
        {
            if (nudgeHarvesterForm == null)
            {
                throw new ArgumentNullException(nameof(nudgeHarvesterForm));
            }
            this.NudgeHarvesterForm = nudgeHarvesterForm;
            this.LoopTimer = new Timer { Interval = Cycle };
            this.LoopTimer.Tick += this.TimerCallback;
            this.LoopTimer.Start();

            this.NudgeHarvesterForm.FormClosing += this.NudgeHarvesterFormOnFormClosing;
        }

        /// <summary>
        /// The nudge harvester form on form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="formClosingEventArgs">
        /// The form closing event args.
        /// </param>
        private void NudgeHarvesterFormOnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            this.LoopTimer.Dispose();
        }

        /// <summary>
        /// The timer callback.
        /// </summary>
        /// <param name="state">
        /// The state.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void TimerCallback(object state, EventArgs e)
        {
            this.myAttentionSpanKnower.Increment(Cycle);
            this.NudgeHarvesterForm.OutputText("Current Foreground App: " + this.myForegroundAppKnower.GetForegroundApp());
            this.NudgeHarvesterForm.OutputText("Mouse Inactive For: " + this.myMouseActivityKnower.GetInactiveMouseElapsed() + "ms");
            this.NudgeHarvesterForm.OutputText("Current Attention Span: " + this.myAttentionSpanKnower.GetAttentionSpan() + "ms");
            this.NudgeHarvesterForm.OutputText(string.Empty);

        }
    }

}
