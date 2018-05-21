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
        string To { get { return nameTo; } }

        public string data;
        string Data { set; get; }

        public Message(string to, string data) {                    
            this.nameTo = to;
            this.data = data;
        }

        public Message(string data)
        {            
            this.data = data;
        }

        override public string ToString() {
            StringBuilder builder = new StringBuilder();            
            builder.Append(data).Append(Delimiter); 
            builder.Append(nameTo);
            return builder.ToString();
        }

        public static Message Parse(string dataString) {
            try
            {
                string[] parts = dataString.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries);
                string data = parts[0];
                if (parts.Length > 1) {
                    string to = parts[1];
                    return new Message(to, data);
                } else
                    return new Message(data);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
