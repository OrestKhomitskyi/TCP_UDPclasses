using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomFake
{
    public class Chat : Connection
    {
        private IPAddress MyIp;

        private int Port = 3334;
        private Task ReceivingTask;
        private CancellationTokenSource ts;
        private UdpClient client;

        public event Action<Message> OnMessage;



        public Chat()
        {
            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            ReceivingTask = new Task(Receive, ct);

        }

        public void SetIp(IPAddress ip)
        {
            MyIp = ip;
            client?.Close();
            client = new UdpClient();
        }

        public async void Start()
        {

            ReceivingTask.Start();

            await ReceivingTask;
        }

        public override void Receive()
        {
            IPAddress ipAddressGroup = GrouIpAddress;

            client.Client.Bind(new IPEndPoint(MyIp, Port));
            client.JoinMulticastGroup(ipAddressGroup);

            IPEndPoint senderIp = null;
            while (true)
            {
                byte[] data = client.Receive(ref senderIp);
                var str = Encoding.UTF8.GetString(data);
                Message Message = new Message() { Data = str, Address = senderIp.Address };
                OnMessage(Message);
            }
        }
        public override void Send(byte[] Bytes)
        {
            if (client == null) return;
            IPEndPoint GroupEndpoint = new IPEndPoint(GrouIpAddress, Port);
            client.Send(Bytes, Bytes.Length, GroupEndpoint);
        }
    }
}
