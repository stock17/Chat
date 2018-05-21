using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Message
    {
        private static string Delimiter = "@^";
        public static char UserDelimiter = '|';
        
        private string nameTo;
        public string To { get { return nameTo; } }

        private string nameFrom;
        public string From { get { return nameFrom; } }

        private string data;
        public string Data
        {
            set { data = value; }
            get { return data;   }
        }

        private int type = 0;
        public int MessageType {
            set { type = value; }
            get { return type;  }
        }
                

        public Message(string data, string from, string to) : this(data, from) {                    
            this.nameTo = to;            
        }

        public Message(string data, string nameFrom)
        {            
            this.data = data;
            this.nameFrom = nameFrom;            
        }

        override public string ToString() {
            StringBuilder builder = new StringBuilder();
            builder.Append(type.ToString());
            builder.Append(Delimiter).Append(data);
            builder.Append(Delimiter).Append(nameFrom);
            if (!String.IsNullOrEmpty(nameTo)) {
                builder.Append(Delimiter).Append(nameTo);
            }
            
            return builder.ToString();
        }

        public static Message Parse(string dataString) {
            try
            {
                Message result;
                string[] parts = dataString.Split(new[] { Delimiter }, StringSplitOptions.None);
                string type = parts[0];
                string data = parts[1];
                string from = parts[2];
                if (parts.Length > 3) {
                    string to = parts[3];
                    result = new Message(data, from, to);
                } else
                    result = new Message(data, from);

                result.MessageType = Int32.Parse(type);
                return result;
            }
            catch (Exception e)
            {
                throw new FormatException();                
            }
        }

        public static class Type
        {
            public const int USUAL_MESSAGE = 0;
            public const int PRIVATE_MESSAGE = 1;
            public const int USER_CONNECTED = 2;
            public const int USER_DISCONNECTED = 3;
            public const int USER_LIST = 4;
        }
    }
}
