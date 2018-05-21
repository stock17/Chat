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
            builder.Append(data);
            builder.Append(Delimiter).Append(nameFrom);
            if (!String.IsNullOrEmpty(nameTo)) {
                builder.Append(Delimiter).Append(nameTo);
            }
            
            return builder.ToString();
        }

        public static Message Parse(string dataString) {
            try
            {
                string[] parts = dataString.Split(new[] { Delimiter }, StringSplitOptions.None);
                string data = parts[0];
                string from = parts[1];
                if (parts.Length > 2) {
                    string to = parts[2];
                    return new Message(data, from, to);
                } else
                    return new Message(data, from);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
