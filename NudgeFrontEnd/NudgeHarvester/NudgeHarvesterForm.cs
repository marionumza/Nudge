using System.Windows.Forms;

namespace NudgeHarvester
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net.Mime;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// The nudge harvester form.
    /// </summary>
    public partial class NudgeHarvesterForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// The harvesterProgram.
        /// </summary>
        private HarvesterProgram harvesterProgram;

        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeHarvesterForm"/> class.
        /// </summary>
        public NudgeHarvesterForm()
        {
            this.InitializeComponent();
            this.Opacity = 0;
            this.harvesterProgram = new HarvesterProgram(this);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Hide();
            this.Opacity = 100;

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

        /// <summary>
        /// The exit tool strip menu item click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.notifyIcon1.Visible = false;
            this.Dispose();
        }

        /// <summary>
        /// The nudge harvester form_ form closing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void NudgeHarvesterFormFormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// The show log tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ShowLogToolStripMenuItemClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// The button 1_ click.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void Button1Click(object sender, EventArgs e)
        {
        }
    }

}
