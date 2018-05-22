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
        public const string SERVER_NAME = "Server";

        private List<ClientHandler> Clients = new List<ClientHandler>();
        private int port = 8000;
        private string ipaddress = "127.0.0.1";
        Thread serverThread;
        Socket serverSocket;

        private bool running = false;



        public void Start()
        {            
            if (running) {
                NotifyAll("Server is running...\n");
                return;
            }                
            
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
            serverSocket.Close();            
            NotifyAll("Conection is closed");
            NotifyAll("Server thread is stopped");

            foreach (ClientHandler ch in Clients) {
                ch.Send(new Message("Server shut down", "Server"));
                ch.Stop();
            }

            Clients.Clear();

            running = false;
        }

        public void Activate()
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);            
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ipaddress), port);
            serverSocket.Bind(ipPoint);
            serverSocket.Listen(10);
            NotifyAll("Server is online.\n IP Adress: " + ipaddress + ". Port: " + port);
            running = true;

            try
            {
                while (true)
                {
                    Socket clientSocket = serverSocket.Accept();

                    byte[] buffer = new byte[1024];
                    int bytesRec = clientSocket.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    Message message = Message.Parse(data);
                    string user = message.From;                        
                    Console.WriteLine(message.Data);

                    ClientHandler client = new ClientHandler(this, clientSocket, user);
                    Clients.Add(client);

                    SendAll(new Message(user + " connected", "Server"));
                    NotifyAll(user + " connected");

                    SendUserList();
                }
            } catch
            {
                NotifyAll("Connection break");
            }      
           
        }

        public void SendAll(Message message) {
            foreach(ClientHandler ch in Clients){
                ch.Send(message);
            }
        }

        public void SendPrivate(Message message)
        {
            SendOne(message, message.To);
            SendOne(message, message.From);            
        }

        public void SendOne(Message message, string user) {            
            var users = from c in Clients
                          where c.User.ToLower().Equals(user.ToLower())
                          select c;

            foreach (ClientHandler ch in users)
            {
                ch.Send(message);
            }
        }

        public void SendUserList()
        {
            StringBuilder builder = new StringBuilder();
            foreach(ClientHandler ch in Clients)
            {
                builder.Append(Message.UserDelimiter).Append(ch.User);
            }
            builder.Remove(0, 1); // remove first delimiter
            Message userMessage = new Message(builder.ToString(), SERVER_NAME);
            userMessage.MessageType = Message.Type.USER_LIST;
            SendAll(userMessage);
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
