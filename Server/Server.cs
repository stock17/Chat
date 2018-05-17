using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server
{
    class Server
    {
        private int port = 8000;

        public void Start()
        {
            Thread thread = new Thread(Activate);
            thread.Start();
        }

        public void Start(int portNumber)
        {
            this.port = portNumber;
            Start();
        }

        public void Activate()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            serverSocket.Bind(ipPoint);
            serverSocket.Listen(10);

            Console.WriteLine("Server started...");

            Socket clientSocket = serverSocket.Accept();
        }
    }
}
