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
        private List<ClientHandler> Clients = new List<ClientHandler>();
        private int port = 8000;
        Thread serverThread;
        

        public void Start()
        {
            serverThread = new Thread(Activate);
            serverThread.Start();
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

            while (true) {
                Socket clientSocket = serverSocket.Accept();
                Console.WriteLine("New client connected...");

                byte[] buffer = Encoding.UTF8.GetBytes("New client connected...");
                int bytesSent = clientSocket.Send(buffer);

                ClientHandler client = new ClientHandler(this, clientSocket);
                Clients.Add(client);
            }           
           
        }

        public void SendAll(string message) {
            foreach(ClientHandler ch in Clients){
                ch.Send(message);
            }
        }
    }
}
