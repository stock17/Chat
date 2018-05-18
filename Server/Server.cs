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
        Socket serverSocket;



        public void Start()
        {
            try
            {
                serverThread = new Thread(Activate);
                serverThread.Start();

                NotifyAll("Server starting...\n");
            }
            catch { }
        }

        public void Start(int portNumber)
        {
            this.port = portNumber;
            Start();
        }

        public void Stop()
        {
            NotifyAll("Server is stopping...");
            //serverSocket.Shutdown(SocketShutdown.Both);
            serverSocket.Close();
            NotifyAll("Conection is closed");

            NotifyAll("Server thread is stopped");
            
            
            Clients.Clear();
        }

        public void Activate()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
            serverSocket.Bind(ipPoint);
            serverSocket.Listen(10);
            NotifyAll("Server is online.\n IP Adress: " + ipaddress + ". Port: " + port);

            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();
                    NotifyAll("New client connected.");

                    ClientHandler client = new ClientHandler(this, clientSocket);
                    Clients.Add(client);
                }
            } catch
            {
                NotifyAll("Connection break");
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
