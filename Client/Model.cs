using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Model
    {
        private Socket socket;
        private int port = 8000;
        private byte[] buffer = new byte[256];
        private Thread listenThread;
        
        
        public Model()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            socket.Connect(remotePoint);
            Console.WriteLine("Successful connection!");

            listenThread = new Thread(StartListening);
            listenThread.Start();
        }

        private void StartListening()
        {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = socket.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    //Console.WriteLine(data);
                    // TODO: send message to Form
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }

        public void Send(string message) {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                int bytesSent = socket.Send(buffer);
            }
            catch (Exception e) { }
        }

    }
}
