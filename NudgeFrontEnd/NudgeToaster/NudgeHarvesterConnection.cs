using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Networking;
using Windows.Networking.Sockets;

namespace NudgeToaster
{
    class NudgeHarvesterConnection
    {
        private DatagramSocket socket;
        private MainPage mainPage;

        public NudgeHarvesterConnection(MainPage page)
        {
            socket = new DatagramSocket();
            this.mainPage = page;
        }

        public async void connectToHarvester()
        {
            socket.Control.DontFragment = true;
            socket.MessageReceived += MessageReceived;

            try
            {
                // Connect to the server (by default, the listener we created in the previous step).
                await socket.ConnectAsync(new HostName("127.0.0.1"), "11012");
                mainPage.output("Connected: " + socket.Information.LocalAddress + ":" + socket.Information.LocalPort);
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

        private void MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                // Interpret the incoming datagram's entire contents as a string.
                uint stringLength = args.GetDataReader().UnconsumedBufferLength;
                string receivedMessage = args.GetDataReader().ReadString(stringLength);

               mainPage.output(
                    "Received data from remote peer: \"" +
                    receivedMessage + "\"");
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
