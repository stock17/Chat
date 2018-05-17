using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class ClientForm : Form
    {
        private Controller controller;

        public ClientForm()
        {
            InitializeComponent();
            controller = new Controller();            
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            string message = messageTextBox.Text.ToString();
            controller.OnSendMessageButton(message);
        }
    }
}
