using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class ClientHandler
    {
        private string userName;
        private Socket socket;
        private Thread listenThread;
        private Server server;

        public string User { get { return userName; } }
        

        public ClientHandler(Server server, Socket clientSocket, string user) {
            this.socket = clientSocket;
            this.server = server;
            this.userName = user;
            listenThread = new Thread(StartListening);
            listenThread.Start();            
        }

        private void StartListening() {
            while (true)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRec = socket.Receive(buffer);
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRec);
                    Message message = Message.Parse(data);
                    Console.WriteLine(message.Data);
                    server.SendAll(message);
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }

        public void Send(Message message) {
            try
            {                
                byte[] buffer = Encoding.UTF8.GetBytes(message.ToString());
                int bytesSent = socket.Send(buffer);                
            }
            catch (Exception e) { }
        }

        public void Stop()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
