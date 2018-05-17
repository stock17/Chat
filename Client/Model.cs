using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Model
    {
        private Socket socket;
        private int port = 8000;
        private byte[] buffer = new byte[256];
        

        public Model()
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            socket.Connect(remotePoint);
            Console.WriteLine("Successful connection!");

        }

        public void Send(string message) {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            socket.Send(bytes);
        }

    }
}
