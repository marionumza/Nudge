using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace NudgeToaster
{
    class NudgeHarvesterConnection
    {
        private DatagramSocket socketListener;
        private MainPage mainPage;
        private DataWriter writer;
        private DatagramSocket socketTalker;

        public NudgeHarvesterConnection(MainPage page)
        {
            socketListener = new DatagramSocket();
            socketTalker = new DatagramSocket();

            this.mainPage = page;
        }

        public void connectToHarvester()
        {
            socketListener.MessageReceived += MessageReceived;


            try
            {
                startListening();

            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }

                mainPage.output("Connect failed with error: " + exception.Message);
            }
        }

        private async void startTalker()
        {
            // Connect to the server
            await socketTalker.ConnectAsync(new HostName("127.0.0.1"), "11111");
            mainPage.output("Connected: " + socketTalker.Information.RemoteAddress + " " + socketTalker.Information.RemotePort);
        }

        private async void startListening()
        {

            // Set local IP
            await socketListener.BindEndpointAsync(new HostName("127.0.0.1"), "22222");
            mainPage.output("Listening: " + socketListener.Information.LocalAddress + " " + socketListener.Information.LocalPort);

        }

        public async void pingHarvester()
        {
            startTalker();
            writer = new DataWriter(socketTalker.OutputStream);
            writer.WriteString("Ping!");

            try
            {
                await writer.StoreAsync();
                mainPage.output("Sent message successfully.");
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error if fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
                mainPage.output("Send failed with error" + exception.Message);

            }
            writer.DetachStream();
            writer.Dispose();
        }

        private async void MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                // Interpret the incoming datagram's entire contents as a string.
                uint stringLength = args.GetDataReader().UnconsumedBufferLength;
                string receivedMessage = args.GetDataReader().ReadString(stringLength);

                mainPage.output(
                     "Received data from remote peer: \"" +
                     receivedMessage + "\"");
                //socketListener.Dispose();
                //startListening();
            }
            catch (Exception exception)
            {
                SocketErrorStatus socketError = SocketError.GetStatus(exception.HResult);
                if (socketError == SocketErrorStatus.ConnectionResetByPeer)
                {
                    // This error would indicate that a previous send operation resulted in an 
                    // ICMP "Port Unreachable" message.
                    Debug.WriteLine(
                        "Peer does not listen on the specific port. Please make sure that you run step 1 first " +
                        "or you have a server properly working on a remote server.");
                }
                else if (socketError != SocketErrorStatus.Unknown)
                {
                    Debug.WriteLine(
                        "Error happened when receiving a datagram: " + socketError.ToString());
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
