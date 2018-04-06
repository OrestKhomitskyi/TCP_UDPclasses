using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ZoomFake_TCP_
{
    class FileChat : GroupMulticast
    {
        private readonly UdpClient Client;
        public event Action<Message> OnMessage;
        private CancellationTokenSource cancellationTokenSource;


        public FileChat(IPAddress IpAddress)
        {
            Client = new UdpClient(new IPEndPoint(IpAddress, this.PortFileChat));
            Client.JoinMulticastGroup(GroupIp);
            cancellationTokenSource = new CancellationTokenSource();
        }
        public void StopChatting()
        {
            cancellationTokenSource.Cancel();
            Client.Close();
        }

        private void Receive()
        {
            BinaryFormatter bf = new BinaryFormatter();
            IPEndPoint remote = null;
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    byte[] data = Client.Receive(ref remote);
                    using (var inputMemoryStream = new MemoryStream(data))
                    {
                        FilePiece deserialized = bf.Deserialize(inputMemoryStream) as FilePiece;

                        if (deserialized.Data.Length == 3)
                        {
                            Message message = new Message()
                            {
                                Address = remote.Address,
                                Data = $"File: {deserialized.FileInfo.Name}"
                            };
                            OnMessage(message);
                        }
                        else
                        {
                            WriteReceivedBytes(deserialized);
                        }
                    }
                }
                catch (SocketException ex)
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }


            }
        }


        public async void ReceiveAsync()
        {
            await Task.Factory.StartNew(Receive, cancellationTokenSource.Token);
        }

        private void WriteReceivedBytes(FilePiece deserialized)
        {
            string filepath = deserialized.FileInfo.Name;
            Debug.WriteLine(filepath);
            using (FileStream fs = new FileStream(filepath, FileMode.Append, FileAccess.Write))
            {
                if (deserialized.Data != null)
                    fs.Write(deserialized.Data, 0, deserialized.Data.Length);
                Debug.WriteLine(fs.Length.ToString());
            }
        }
        public async void SendFile(FileInfo FileInfo)
        {

            await Task.Run(() =>
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                IPEndPoint GroupEndPoint = new IPEndPoint(GroupIp, PortFileChat);


                using (FileStream fs = new FileStream(FileInfo.FullName, FileMode.Open, FileAccess.Read))
                using (BinaryReader br = new BinaryReader(fs))
                {
                    MemoryStream ms = new MemoryStream();

                    byte[] data = br.ReadBytes(1024);
                    FilePiece fp = new FilePiece(FileInfo, data);
                    binaryFormatter.Serialize(ms, fp);
                    Thread.Sleep(10);

                    while (data.Length > 0)
                    {
                        Client.Send(ms.GetBuffer(), (int)ms.Length, GroupEndPoint);
                        data = br.ReadBytes(1024);
                        fp.Data = data;
                        ms = new MemoryStream();
                        binaryFormatter.Serialize(ms, fp);
                        Thread.Sleep(10);
                    }
                    Thread.Sleep(10);
                    ms = new MemoryStream();
                    fp.Data = new byte[] { 1, 2, 3 };
                    binaryFormatter.Serialize(ms, fp);
                    Client.Send(ms.GetBuffer(), (int)ms.Length, GroupEndPoint);
                }
            });
        }

    }
}