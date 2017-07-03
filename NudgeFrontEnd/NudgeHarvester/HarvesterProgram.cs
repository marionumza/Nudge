using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NudgeHarvester
{
    using System;
    using System.Windows.Forms;
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
        /// Gets the nudge harvester form.
        /// </summary>
        public NudgeHarvesterForm NudgeHarvesterForm { get; }

        /// <summary>
        /// Gets or sets the loop timer.
        /// </summary>
        public Timer LoopTimer { get; set; }


        private int talkPort;

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
        private async void TimerCallback(object state, EventArgs e)
        {
            this.myAttentionSpanKnower.Increment(Cycle);
            this.NudgeHarvesterForm.OutputText("Current Foreground App: " + this.myForegroundAppKnower.GetForegroundApp());
            this.NudgeHarvesterForm.OutputText("Mouse Inactive For: " + this.myMouseActivityKnower.GetInactiveMouseElapsed() + "ms");
            this.NudgeHarvesterForm.OutputText("Keyboard Inactive For: " + this.myKeyboardActivityKnower.GetInactiveKeyboardElapsed() + "ms");
            this.NudgeHarvesterForm.OutputText("Current Attention Span: " + this.myAttentionSpanKnower.GetAttentionSpan() + "ms");
            this.NudgeHarvesterForm.OutputText(string.Empty);
            sendToClients(this.myForegroundAppKnower.GetForegroundApp());
        }

        private IPEndPoint sending_end_point;
        Boolean exception_thrown = false;
        private UdpClient sending_socket;
        private UdpClient listener;


        public async void startUdpServer(int listenPort, int talkPort)
        {

            // Setup UDP Server 
            listener = new UdpClient(listenPort);
            listener.DontFragment = true;

            this.talkPort = talkPort;

            this.NudgeHarvesterForm.OutputText("Started listening on port: " + listener.Client.AddressFamily.ToString());
            await startListening();

        }

        private async Task startListening()
        {
            try
            {
                UdpReceiveResult receiveResult = await listener.ReceiveAsync();
                this.NudgeHarvesterForm.OutputText(">> Received: " + Encoding.ASCII.GetString(receiveResult.Buffer));
                startListening();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        IPAddress localhost = IPAddress.Parse("127.0.0.1");
        public async void sendToClients(string message)
        {
            // Setup UDP Talker
            //sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //talkPort = 11123;
            sending_end_point = new IPEndPoint(localhost, talkPort);
            byte[] send_buffer = Encoding.ASCII.GetBytes(message);
            //SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            ////args.RemoteEndPoint = sending_end_point;
            //args.SetBuffer(send_buffer, 0, send_buffer.Length);
            //args.DisconnectReuseSocket = false;

            //sending_socket.Bind(sending_end_point);
            sending_socket = new UdpClient();
            sending_socket.Connect(sending_end_point);
            try
            {
                await sending_socket.SendAsync(send_buffer, send_buffer.Length);
                //sending_socket.SendToAsync(args);
                this.NudgeHarvesterForm.OutputText("<< Sending to address:" + sending_end_point.Address + " port: " + sending_end_point.Port);
            }
            catch (Exception send_exception)
            {
                exception_thrown = true;
                this.NudgeHarvesterForm.OutputText(" Exception " + send_exception.Message);
            }


            if (exception_thrown == false)
            {
                Console.WriteLine("Message has been sent to the broadcast address");
            }
            else
            {
                exception_thrown = false;
                Console.WriteLine("The exception indicates the message was not sent.");
            }
        }

        //private async void sendToCallback(IAsyncResult ar)
        //{
        //    sending_socket.EndSendTo(ar);
        //}
    }

}
