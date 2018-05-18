using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Controller
    {

        private Server server;
     
        public Controller(Server server)
        {
            this.server = server;            
        }  

        public void OnStartButton()
        {            
            server.Start();
        }

        public void OnStopButton()
        {
            server.Stop();
        }
    }
}
