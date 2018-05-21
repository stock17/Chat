using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server;


namespace Client
{
    class Model
    {
        private string userName;

        private Socket socket;
        private int port = 8000;
        private byte[] buffer = new byte[256];
        private Thread listenThread;
        private List<string> users = new List<string>();

        private static string CONNECTION_ERROR = "Disconnected";

                
        public Model()
        {
            
        }

        public void Connect(string userName)
        {

            this.userName = userName;            

            try {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
                                
                socket.Connect(remotePoint);
                Console.WriteLine("Successful connection!");

                listenThread = new Thread(StartListening);
                listenThread.Start();

                Send("connected."); // TODO: remake to service message
            }
            catch
            {
                if (listenThread != null)
                    listenThread.Abort();
            }
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
                    Message msg = Message.Parse(data);

                    switch (msg.MessageType)  {

                        case Message.Type.USUAL_MESSAGE:
                            NotifyAll(msg.From + ": " + msg.Data);
                            break;
                        case Message.Type.USER_LIST:
                            users = ExtractUsers(msg);
                            NotifyAll(users);
                            break;
                    }                    
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
                Console.WriteLine(new Message(message, userName).ToString());
                byte[] buffer = Encoding.UTF8.GetBytes(new Message(message, userName).ToString());
                if (socket.Connected)
                {
                    int bytesSent = socket.Send(buffer);
                }
                else
                {
                    NotifyAll(CONNECTION_ERROR);
                }               

            }
            catch (Exception e) { }
        }

        public List<string> ExtractUsers (Message message)
        {
            if (message.MessageType != Message.Type.USER_LIST)
                throw new FormatException();

            List<string> result = new List<string>();
            result = message.Data.Split(Message.UserDelimiter).ToList();
            return result;
        }

        //========= Listeners ============//

        public interface MessageListener
        {
            void  Update(string message);
            void Update(List<string> users);
        }

        private List<MessageListener> listeners = new List<MessageListener>();

        public void AddListener(MessageListener listener)
        {
            listeners.Add(listener);
        }

        public void RemoveListener(MessageListener listener)
        {
            listeners.Remove(listener);
        }

        public void NotifyAll(string newMessage)
        {
            foreach (MessageListener ml in listeners) {
                ml.Update(newMessage);                
            }
        }

        public void NotifyAll(List<string> users)
        {
            foreach (MessageListener ml in listeners)
            {
                ml.Update(users);
            }
        }

    }
}
