using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        private static string Delimiter = "@^";

        private bool serviceType; // 1 - service, 0 - text
        bool IsService() { return serviceType; }

        private bool privateType; // 1 - private, 0 - public
        bool IsPrivate() { return privateType; }

        private string nameFrom;
        string From { get { return nameFrom; } }

        private string nameTo;
        string To { get { return nameTo; } }

        public string data;
        string Data { set; get; }

        public Message(bool isService, bool isPrivate, string from, string to, string data) {
            this.serviceType = isService;
            this.privateType = isPrivate;
            this.nameFrom = from;
            this.nameTo = to;
            this.data = data;
        }

        override public string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append(serviceType.ToString()).Append(Delimiter);
            builder.Append(privateType.ToString()).Append(Delimiter);
            builder.Append(nameFrom).Append(Delimiter);
            builder.Append(nameTo).Append(Delimiter);
            builder.Append(data);
            return builder.ToString();
        }

        public static Message Parse(string dataString) {
            try
            {
                string[] parts = dataString.Split(new[] { Delimiter }, StringSplitOptions.None);
                bool serv = Boolean.Parse(parts[0]);
                bool priv = Boolean.Parse(parts[1]);
                string from = parts[2];
                string to = parts[3];
                string data = parts[4];
                return new Message(serv, priv, from, to, data);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
