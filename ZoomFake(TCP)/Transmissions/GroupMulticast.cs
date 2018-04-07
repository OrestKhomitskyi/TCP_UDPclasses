using System.Net;
using ZoomFake_TCP_.Properties;

namespace ZoomFake_TCP_
{
    public abstract class GroupMulticast
    {
        protected IPAddress GroupIp = IPAddress.Parse(Settings.Default.ipGroup);
        protected int PortChat = Settings.Default.portGroupChat;
        protected int PortFileChat = Settings.Default.portGroupFile;
        protected int PortScreenCast = Settings.Default.portGroupCast;
        protected int PortWebCamCast = Settings.Default.portGroupWebCam;

    }
}
