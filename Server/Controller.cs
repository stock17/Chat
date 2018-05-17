using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Controller
    {

        private Server server;
     
        public Controller()
        {
            server = new Server();
        }  

        public void OnStartButton()
        {
            server.Start();
        }
    }
}
