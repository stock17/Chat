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
    public partial class ClientForm : Form, Model.MessageListener
    {
        private Controller controller;
        private string privateReceiver;

        public ClientForm()
        {
            InitializeComponent();            
        }
        
        private void sendButton_Click(object sender, EventArgs e)
        {
            string message = messageTextBox.Text.ToString();
            messageTextBox.Clear();
            if (message.StartsWith("private to")){
                int i = message.IndexOf(':');
                string text = message.Substring(i + 1);
                controller.OnSendPrivateMessageButton(text, privateReceiver); 
            } else {
                controller.OnSendMessageButton(message);
            }            
            messageTextBox.Focus();
        }

        public void Update(string message)
        {            
            messageListBox.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                messageListBox.Items.Add(message);
                messageListBox.TopIndex = messageListBox.Items.Count - 1;
            });            
        }

        public void Update(List<string> users)
        {
            usersListBox.Invoke((MethodInvoker)delegate {
                // Running on the UI thread
                usersListBox.Items.Clear();
                foreach (String user in users)
                {
                    usersListBox.Items.Add(user);
                }
                
            });
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            if (loginForm.DialogResult == DialogResult.OK)
            {
                string user = loginForm.UserName;
                Model model = new Model(user);
                model.AddListener(this);
                controller = new Controller(model);
                this.Text += " - " + user;
                controller.OnConnectButton();
            } else
            {
                Close();
            }
        }

        private void usersListBox_DoubleClick(object sender, EventArgs e)
        {
            string user = usersListBox.GetItemText(usersListBox.SelectedItem);
            privateReceiver = user;
            messageTextBox.Text = "private to '" + user + "' : ";
            messageTextBox.Focus();
            messageTextBox.SelectionStart = messageTextBox.Text.Length;
        }
    }
}
