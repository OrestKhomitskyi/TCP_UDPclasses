using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using ZoomFake;

namespace ZoomFake_TCP_
{
    public class WebCamCast : GroupMulticast
    {
        private readonly UdpClient ClientSend;
        private readonly UdpClient ClientReceive;


        public event Action<ImageBrush> OnFrameChange;
        protected Task ReceivingTask;
        protected Task SendingTask;
        protected IPEndPoint EndPointGroup;
        protected CancellationTokenSource cancellationTokenSource;


        public WebCamCast(IPAddress IpAddress)
        {
            EndPointGroup = new IPEndPoint(GroupIp, PortWebCamCast);
            ClientReceive = new UdpClient(new IPEndPoint(IPAddress.Any, PortWebCamCast));
            ClientReceive.JoinMulticastGroup(GroupIp);

            ClientSend = new UdpClient();
            ClientSend.JoinMulticastGroup(GroupIp);
            ClientSend.Connect(EndPointGroup);
            cancellationTokenSource = new CancellationTokenSource();

        }




        private byte[] AddImagePiece(byte[] currentdata, byte[] newdata)
        {

            byte[] current = currentdata;
            Array.Resize(ref current, current.Length + newdata.Length);
            Array.Copy(newdata, 0, current, currentdata.Length, newdata.Length);
            return current;
        }


        public void Stop()
        {
            cancellationTokenSource.Cancel();
            ClientReceive.Close();
            ClientSend.Close();
        }


        public async void ReceiveAsync()
        {
            ReceivingTask = new Task(Receive, cancellationTokenSource.Token);
            ReceivingTask.Start();
            await ReceivingTask;
        }
        public async void SendAsync()
        {
            SendingTask = new Task(Send, cancellationTokenSource.Token);
            SendingTask.Start();
            await SendingTask;
        }

        private void Send()
        {
            using (WebCam webCam = new WebCam())
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    try
                    {
                        byte[] screenshotBytes = webCam.GetVideoCaptureBytes();
                        Guid id = Guid.NewGuid();

                        MemoryStream ms = new MemoryStream();
                        BinaryFormatter bf = new BinaryFormatter();

                        using (var inputMs = new MemoryStream(screenshotBytes))
                        using (BinaryReader br = new BinaryReader(inputMs))
                        {
                            FramePieceInfo fi =
                                new FramePieceInfo(br.ReadBytes(64000), id) { TotalLength = screenshotBytes.Length };

                            bf.Serialize(ms, fi);
                            //Thread.Sleep(15);
                            while (fi.FrameBytes.Length > 0)
                            {
                                ClientSend.Send(ms.GetBuffer(), (int)ms.Length);
                                ms = new MemoryStream();
                                fi.FrameBytes = br.ReadBytes(64000);
                                bf.Serialize(ms, fi);
                                Thread.Sleep(15);
                            }
                        }

                        Debug.WriteLine($"Sent: {screenshotBytes.Length}");
                        Thread.Sleep(10);
                    }
                    catch (ObjectDisposedException oex)
                    {

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void Receive()
        {
            IPEndPoint ip = null;
            BinaryFormatter bf = new BinaryFormatter();

            //Default Frame fixes guid and length
            FramePieceInfo CurrentPieceInfo = null;
            FramePieceInfo fpi;
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                try
                {
                    byte[] data = ClientReceive.Receive(ref ip);
                    using (MemoryStream ms = new MemoryStream(data))
                    {
                        fpi = bf.Deserialize(ms) as FramePieceInfo;
                        Debug.WriteLine(ms.Length);
                    }
                    Debug.WriteLine($"\nReceived: {fpi.Id}");
                    Debug.WriteLine($"Received: {fpi.FrameBytes.Length}");
                    Debug.WriteLine($"Total: {fpi.TotalLength}");

                    if (CurrentPieceInfo == null)
                    {
                        CurrentPieceInfo = new FramePieceInfo(fpi.FrameBytes, fpi.Id) { TotalLength = fpi.TotalLength };
                    }
                    else if (CurrentPieceInfo.Id == fpi.Id && CurrentPieceInfo.FrameBytes.Length <= CurrentPieceInfo.TotalLength)
                    {
                        //Merging received and current frame
                        byte[] merged = AddImagePiece(CurrentPieceInfo.FrameBytes, fpi.FrameBytes);
                        CurrentPieceInfo.FrameBytes = merged;
                    }
                    else
                    {
                        CheckForHit(CurrentPieceInfo);
                        CurrentPieceInfo = new FramePieceInfo(fpi.FrameBytes, fpi.Id) { TotalLength = fpi.TotalLength };
                    }
                    CheckForHit(CurrentPieceInfo);
                    Debug.WriteLine("Buffer: " + CurrentPieceInfo?.FrameBytes.Length);
                }
                catch (SocketException ex)
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CheckForHit(FramePieceInfo CurrentPieceInfo)
        {
            if (CurrentPieceInfo.FrameBytes.Length == CurrentPieceInfo.TotalLength)
            {
                Debug.WriteLine("Hit");
                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        OnFrameChange(new ImageBrush(Screenshot.ByteToBitMapSource(CurrentPieceInfo.FrameBytes)));
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
    }
}
