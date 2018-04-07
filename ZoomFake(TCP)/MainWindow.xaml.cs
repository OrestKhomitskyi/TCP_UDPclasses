using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZoomFake_TCP_.Properties;

namespace ZoomFake_TCP_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool IsConnected { get; set; }
        private bool IsScreenCasting = false;
        private bool IsWebCamCasting = false;

        private GroupChat GroupChat;
        private FileChat FileChat;
        private ScreenCast ScreenCast;
        private WebCamCast WebCamCast;


        public MainWindow()
        {
            InitializeComponent();
            IsConnected = false;
            Ip.Text = Settings.Default.localIp;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (!IsConnected)
                {
                    GroupChat = new GroupChat(IPAddress.Parse(Ip.Text));
                    FileChat = new FileChat(IPAddress.Parse(Ip.Text));

                    GroupChat.OnMessage += GroupChat_OnMessage;
                    FileChat.OnMessage += FileChat_OnMessage;

                    GroupChat.ReceiveAsync();
                    FileChat.ReceiveAsync();
                }
                else
                    CloseConnection();

                IsConnected = !IsConnected;
                ScreenCastGrid.IsEnabled = IsConnected;
                GridChats.IsEnabled = IsConnected;
                ChangeButtonState((Button)sender);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CloseConnection()
        {
            GroupChat.StopChat();
            FileChat.StopChatting();

            ScreenCast?.Stop();
        }


        private void FileChat_OnMessage(Message obj)
        {
            GroupChat_OnMessage(obj);
        }

        private void GroupChat_OnMessage(Message obj)
        {
            Dispatcher.Invoke(() =>
            {
                ListViewChat.Items.Add(obj);
            });
        }

        private void ChangeButtonState(Button btn)
        {
            if (IsConnected)
            {
                btn.Content = "Disconnect";
            }
            else
            {
                btn.Content = "Connect";
            }
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            switch (e.Key)
            {
                case Key.Enter:
                    GroupChat.Send(Encoding.UTF8.GetBytes(ChatTextBox.Text));
                    ChatTextBox.Text = "";
                    break;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GroupChat.Send(Encoding.UTF8.GetBytes(ChatTextBox.Text));
            ChatTextBox.Text = "";
        }

        private void ButtonBase_OnClick(object Sender, RoutedEventArgs E)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                FileChat.SendFile(new FileInfo(filename));
            }
        }

        private void ShareWebCam_OnClick(object Sender, RoutedEventArgs E)
        {
            IsWebCamCasting = !IsWebCamCasting;
            WebCamCast?.Stop();

            if (IsWebCamCasting)
            {
                WebCamCast = new WebCamCast(IPAddress.Parse(Ip.Text));
                WebCamCast.SendAsync();
                ShareWebCam.Content = "Stop";
                ReceiveWebCam.IsEnabled = false;
            }
            else
            {
                ShareWebCam.Content = "Share WebCam";
                ReceiveWebCam.IsEnabled = true;
            }
        }


        private void ReceiveWebCam_OnClick(object Sender, RoutedEventArgs E)
        {
            WebCamCast?.Stop();
            WebCamCast = new WebCamCast(IPAddress.Parse(Ip.Text));
            ScreenCastWindow scw = new ScreenCastWindow();

            WebCamCast.OnFrameChange += (s) =>
            {
                scw.Dispatcher.Invoke(() =>
                {
                    scw.ScreenCast_OnFrameChange(s);
                });
            };
            scw.Show();
            WebCamCast.ReceiveAsync();
        }

        private void ShareScreen_OnClick(object Sender, RoutedEventArgs E)
        {
            IsScreenCasting = !IsScreenCasting;
            ScreenCast?.Stop();

            if (IsScreenCasting)
            {
                ScreenCast = new ScreenCast(IPAddress.Parse(Ip.Text));
                ScreenCast.SendAsync();
                ShareScreen.Content = "Stop";
                ReceiveScreen.IsEnabled = false;
            }
            else
            {
                ShareScreen.Content = "Share Screen";
                ReceiveScreen.IsEnabled = true;
            }
        }

        private void ReceiveScreen_OnClick(object Sender, RoutedEventArgs E)
        {
            ScreenCast?.Stop();
            ScreenCast = new ScreenCast(IPAddress.Parse(Ip.Text));
            ScreenCastWindow scw = new ScreenCastWindow();

            ScreenCast.OnFrameChange += (s) =>
            {
                scw.Dispatcher.Invoke(() =>
                {
                    scw.ScreenCast_OnFrameChange(s);
                });
            };
            scw.Show();
            ScreenCast.ReceiveAsync();
        }
    }
}
