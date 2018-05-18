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
        private string ipaddress = "127.0.0.1";
        Thread serverThread;
        

        public void Start()
        {
            serverThread = new Thread(Activate);
            serverThread.Start();

            NotifyAll("Server starting...\n");
        }

        public void Start(int portNumber)
        {
            this.port = portNumber;
            Start();
        }

        public void Activate()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
            serverSocket.Bind(ipPoint);
            serverSocket.Listen(10);
            NotifyAll("Server is online.\n IP Adress: " + ipaddress + ". Port: " + port + "\n");

            while (true) {
                Socket clientSocket = serverSocket.Accept();
                NotifyAll("New client connected.");

                ClientHandler client = new ClientHandler(this, clientSocket);
                Clients.Add(client);                
            }           
           
        }

        public void SendAll(string message) {
            foreach(ClientHandler ch in Clients){
                ch.Send(message);
            }
        }

        //========= Listeners ============//

        public interface StatusListener
        {
            void Update(string message);
        }

        private List<StatusListener> listeners = new List<StatusListener>();

        public void AddListener(StatusListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(StatusListener listener)
        {
            listeners.Remove(listener);
        }

        public void NotifyAll(string newMessage)
        {
            foreach (StatusListener sl in listeners)
            {
                sl.Update(newMessage);
            }
        }
    }
}
