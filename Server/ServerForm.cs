using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class ServerForm : Form, Server.StatusListener
    {

        Controller controller;

        public ServerForm()
        {
            InitializeComponent();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            Server server = new Server();
            server.AddListener(this);
            controller = new Controller(server);
            controller.OnStartButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            controller.OnStartButton();
        }

        public void Update(string message)
        {
            statusListBox.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                statusListBox.Items.Add(message + "\n");
            });
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            controller.OnStopButton();
        }
    }
}
