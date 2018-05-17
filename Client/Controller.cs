using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Controller
    {
        Model model;

        public Controller()
        {
            model = new Model();
        }

        public void OnSendMessageButton(string message) {
            model.Send(message);
        }

    }
}
