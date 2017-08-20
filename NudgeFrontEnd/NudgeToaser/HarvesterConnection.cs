using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NudgeToaser
{

    class HarvesterConnection
    {
        private IPEndPoint sending_end_point;
        Boolean exception_thrown = false;
        private UdpClient sending_socket;
        private UdpClient listener;
        IPAddress talkAddress = IPAddress.Parse("127.0.0.1");
        private int listenPort = 22222;
        private int talkPort = 11111;

        public async void connectToHarvester()
        {
            // Setup UDP Server 
            listener = new UdpClient(listenPort);

            sending_socket = new UdpClient();
            sending_socket.DontFragment = true;
            sending_end_point = new IPEndPoint(talkAddress, this.talkPort);
            sending_socket.Connect(sending_end_point);

            Console.WriteLine("Started listening on port: " + listener.Client.AddressFamily.ToString());
            await startListening();
        }

        private async Task startListening()
        {
            try
            {
                UdpReceiveResult receiveResult = await listener.ReceiveAsync();
                Console.WriteLine(">> Received: " + Encoding.ASCII.GetString(receiveResult.Buffer));
                startListening();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
