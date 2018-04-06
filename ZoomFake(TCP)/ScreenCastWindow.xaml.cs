using System.Windows;
using System.Windows.Media;

namespace ZoomFake_TCP_
{
    /// <summary>
    /// Interaction logic for ScreenCastWindow.xaml
    /// </summary>
    public partial class ScreenCastWindow : Window
    {


        public ScreenCastWindow()
        {
            InitializeComponent();
        }

        public void ScreenCast_OnFrameChange(ImageBrush obj)
        {
            Grid.Background = obj;
        }

    }
}
