﻿using System;
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
        private Socket socket;
        private Thread listenThread;
        private Server server;

        public ClientHandler(Server server, Socket clientSocket) {
            this.socket = clientSocket;
            this.server = server;
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
                    Console.WriteLine(data);
                    server.SendAll(data);
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