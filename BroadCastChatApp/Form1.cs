using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BroadCastChatApp
{
    public partial class Form1 : Form
    {
        private const string Address = "25.83.118.192";
        private const string BroadCastAddress = "25.255.255.255";
        private UdpClient client;
        private List<Message> BlackList = new List<Message>();

        public Form1()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            client = new UdpClient(new IPEndPoint(IPAddress.Parse(Address), 3333));

            //Очікуємо
            Task.Run(() =>
            {
                IPEndPoint ep = null;
                while (true)
                {
                    byte[] get_data = client.Receive(ref ep);

                    Message message = new Message() { Address = ep.Address, Data = Encoding.UTF8.GetString(get_data) };

                    //Muting User
                    if (BlackList.Where(x => x.Address == message.Address).Count() == 0)
                    {
                        listBox1.Items.Add(message);
                    }
                }
            });

        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Parse(BroadCastAddress), 3333);
            //Message
            string message = "Orest sent: " + textBox1.Text;
            client.Send(Encoding.UTF8.GetBytes(message), message.Length, remote);
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            Message user = (Message)this.listBox1.SelectedItem;
            PrivateChat chat = new PrivateChat(client, user);
            chat.ShowDialog();
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            Message danger = (Message)this.listBox1.SelectedItem;

            //Добавити в чорний список якщо пустий
            //if (BlackList.Where(x=>x.Address==danger.Address).Count()==0)
            BlackList.Add(danger);

        }
    }
}
