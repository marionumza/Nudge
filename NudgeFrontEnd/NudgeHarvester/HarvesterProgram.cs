

namespace NudgeHarvester
{
    using NudgeUtilities;
    using System;
    using System.IO;
    using System.Windows.Forms;

    using CsvHelper;

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
        /// The current harvest.
        /// </summary>
        private Harvest currentHarvest = new Harvest();

        /// <summary>
        /// The csv stream.
        /// </summary>
        private StreamWriter csvStream = new StreamWriter(Path.GetTempPath() + "HARVEST.CSV");

        /// <summary>
        /// The csv writer.
        /// </summary>
        private CsvWriter csvWriter;


        /// <summary>
        /// Gets or sets the loop timer.
        /// </summary>
        public Timer LoopTimer { get; set; }



        /// <summary>
        /// Initializes a new instance of the <see cref="HarvesterProgram"/> class.
        /// </summary>
        /// <param name="nudgeHarvesterForm">The Form</param>
        public HarvesterProgram(NudgeHarvesterForm nudgeHarvesterForm)
        {

            this.csvWriter = new CsvWriter(csvStream);
            this.NudgeHarvesterForm = nudgeHarvesterForm ?? throw new ArgumentNullException(nameof(nudgeHarvesterForm));

            this.udpEngine = new UdpEngine(22222, 11111, CallbackAsync);
            this.udpEngine.StartUdpServer();

            this.LoopTimer = new Timer { Interval = Cycle };
            this.LoopTimer.Tick += this.TimerCallback;
            this.LoopTimer.Start();



            Application.ApplicationExit += (sender, args) =>
            {
                this.LoopTimer.Dispose();
                this.csvWriter.Dispose();
                this.csvStream.Close();
            };
        }

        /// <summary>
        /// The callback.
        /// </summary>
        /// <param name="received">
        /// The received.
        /// </param>
        private void CallbackAsync(string received)
        {
            if (received.Equals("PAUSE"))
            {
                this.LoopTimer.Stop();
            }
            else if (received.Equals("SAVE"))
            {
                this.SaveCsvAsync().ConfigureAwait(false);
            }
            else if (received.Equals("RESUME"))
            {
                this.LoopTimer.Start();
            }
        }

        /// <summary>
        /// Gets the nudge harvester form.
        /// </summary>
        private NudgeHarvesterForm NudgeHarvesterForm { get; }

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

            this.currentHarvest.AttentionSpan = this.myAttentionSpanKnower.GetAttentionSpan();
            this.currentHarvest.ForegroundAppHash = this.myForegroundAppKnower.GetForegroundApp().GetHashCode();
            this.currentHarvest.MouseActivity = this.myMouseActivityKnower.GetInactiveMouseElapsed();
            this.currentHarvest.KeyboardActivity = this.myKeyboardActivityKnower.GetInactiveKeyboardElapsed();


            this.NudgeHarvesterForm.OutputText("Current Foreground App: " + this.myForegroundAppKnower.GetForegroundApp());
            this.NudgeHarvesterForm.OutputText("Current Foreground App Int: " + this.currentHarvest.ForegroundAppHash);
            this.NudgeHarvesterForm.OutputText("Mouse Inactive For: " + this.currentHarvest.MouseActivity + "ms");
            this.NudgeHarvesterForm.OutputText("Keyboard Inactive For: " + this.currentHarvest.KeyboardActivity + "ms");
            this.NudgeHarvesterForm.OutputText("Current Attention Span: " + this.currentHarvest.AttentionSpan + "ms");
            this.NudgeHarvesterForm.OutputText(string.Empty);
            this.udpEngine.SendToClients("Ping!");
        }

        /// <summary>
        /// The save harvest to csv.
        /// </summary>
        private async System.Threading.Tasks.Task SaveCsvAsync()
        {
            this.csvWriter.WriteRecord(this.currentHarvest);
            await this.csvStream.FlushAsync().ConfigureAwait(false);
        }
    }
}
