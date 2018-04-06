using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadCastChatApp
{
    public partial class PrivateChat : Form
    {
        private IPAddress address;
        private UdpClient me;
        public PrivateChat(UdpClient me,Message dataUser)
        {
            InitializeComponent();
            address = dataUser.Address;
            this.me = me;
            this.label1.Text = dataUser.Address.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Parse(address.ToString()), 3333);
            //Message
            string message = "Orest sent private: " + textBox1.Text;

            me.Send(Encoding.UTF8.GetBytes(message), message.Length, remote);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
