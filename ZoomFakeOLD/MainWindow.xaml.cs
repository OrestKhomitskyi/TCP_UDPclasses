using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ZoomFake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ScreenCast ScreenCast;
        private Chat Chat;
        public MainWindow()
        {
            InitializeComponent();
            Chat = new Chat();
            Chat.OnMessage += Chat_OnMessage;

        }

        private void Chat_OnMessage(Message obj)
        {
            Dispatcher.Invoke(() => { ChatListBox.Items.Add(obj); });
        }

        private void StartItem_Click(object sender, RoutedEventArgs e)
        {
            ScreenCast?.Receive();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Send_OnClick(object Sender, RoutedEventArgs E)
        {
            ScreenCast.StartCasting();

        }

        private void ButtonBase_OnClick(object Sender, RoutedEventArgs E)
        {
            ScreenCast = new ScreenCast(IpTextBox.Text);
            Chat.SetIp(IPAddress.Parse(IpTextBox.Text));

            Chat.Start();
            //ScreenCast.SetReceivingMode += () => { Dispatcher.Invoke(() => { Screen.Content = "Casting"; }); };


            ScreenCast.FrameChange += Source =>
            {
                Dispatcher.Invoke(() =>
                {
                    try
                    {
                        //Debug.WriteLine(Source.Length);
                        Screen.Background = new ImageBrush(Screenshot.ByteToBitMapSource(Source));
                    }
                    catch (Exception ex)
                    {

                    }
                });
            };
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void TextMessage_OnKeyDown(object Sender, KeyEventArgs E)
        {
            if (E.Key == Key.Enter && TextMessage.Text != "")
                Chat.Send(Encoding.UTF8.GetBytes($"Orest: {TextMessage.Text}"));
        }
    }
}
