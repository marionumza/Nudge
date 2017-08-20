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
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class NudgeHarvesterForm : System.Windows.Forms.Form
    {
        private HarvesterProgram havester;

        /// <summary>
        /// Initializes a new instance of the <see cref="NudgeHarvesterForm"/> class.
        /// </summary>
        public NudgeHarvesterForm()
        {
            this.InitializeComponent();
            havester = new HarvesterProgram(this);
            havester.startUdpServer();
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Dispose();
        }

        private void showLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

        }

        private void NudgeHarvesterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            havester.sendToClients("Hello");
        }
    }

}
