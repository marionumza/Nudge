// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UdpEngine.cs" company="">
//   
// </copyright>
// <summary>
//   The udp engine.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace NudgeUtilities
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The udp engine.
    /// </summary>
    public class UdpEngine
    {

        /// <summary>
        /// The talk port.
        /// </summary>
        private readonly int talkPort;

        /// <summary>
        /// The listen port.
        /// </summary>
        private readonly int listenPort;

        /// <summary>
        /// The talkIpEndPoint.
        /// </summary>
        private IPEndPoint talkIpEndPoint;

        /// <summary>
        /// The exception thrown on send.
        /// </summary>
        private bool exceptionThrown;

        /// <summary>
        /// The talkUdpClient.
        /// </summary>
        private UdpClient talkUdpClient;

        /// <summary>
        /// The listenerUdpClient.
        /// </summary>
        private UdpClient listenerUdpClient;

        /// <summary>
        /// The talk address.
        /// </summary>
        private IPAddress talkAddress = IPAddress.Parse("127.0.0.1");

        private RecieveDelegate receiveCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="UdpEngine"/> class.
        /// </summary>
        /// <param name="talkPort">
        /// The talk port.
        /// </param>
        /// <param name="listenPort">
        /// The listen port.
        /// </param>
        public UdpEngine(int talkPort, int listenPort, RecieveDelegate callback)
        {
            this.talkPort = talkPort;
            this.listenPort = listenPort;
            this.receiveCallback = callback;
        }

        /// <summary>
        /// The start udp server.
        /// </summary>
        public async void StartUdpServer()
        {

            // Setup UDP Server 
            this.listenerUdpClient = new UdpClient(this.listenPort);

            this.talkUdpClient = new UdpClient();
            this.talkUdpClient.DontFragment = true;
            this.talkIpEndPoint = new IPEndPoint(this.talkAddress, this.talkPort);
            this.talkUdpClient.Connect(this.talkIpEndPoint);

            Console.WriteLine("Started listening on port: " + this.listenerUdpClient.Client.AddressFamily.ToString());
            await this.StartListeningAsync();

        }

        public delegate void RecieveDelegate(string received);

        /// <summary>
        /// The start listening.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task StartListeningAsync()
        {
            try
            {
                UdpReceiveResult receiveResult = await this.listenerUdpClient.ReceiveAsync();
                string receivedString = Encoding.ASCII.GetString(receiveResult.Buffer);
                receiveCallback(receivedString);
                Console.WriteLine(">> Received: " + receivedString);
                await this.StartListeningAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// The send to clients.
        /// </summary>
        /// <param name="message">
        /// The message. 
        /// </param>
        public async void SendToClients(string message)
        {
            // Setup UDP Talker
            byte[] sendBuffer = Encoding.ASCII.GetBytes(message);
            try
            {
                await this.talkUdpClient.SendAsync(sendBuffer, sendBuffer.Length);
                Console.WriteLine("<< Sending to address:" + this.talkIpEndPoint.Address + " port: " + this.talkIpEndPoint.Port);
            }
            catch (Exception sendException)
            {
                this.exceptionThrown = true;
                Console.WriteLine(" Exception " + sendException.Message);
            }


            if (this.exceptionThrown == false)
            {
                Console.WriteLine("Message has been sent to the broadcast address");
            }
            else
            {
                this.exceptionThrown = false;
                Console.WriteLine("The exception indicates the message was not sent.");
            }
        }
    }

}
