using System.Configuration;
using System.Net;

namespace ZoomFake_TCP_
{
    public abstract class GroupMulticast
    {
        protected IPAddress GroupIp = IPAddress.Parse(ConfigurationManager.AppSettings.Get("ipGroup").ToString());
        protected int PortChat = int.Parse(ConfigurationManager.AppSettings.Get("portGroupChat").ToString());
        protected int PortFileChat = int.Parse(ConfigurationManager.AppSettings.Get("portGroupFile").ToString());
        protected int PortScreenCast = int.Parse(ConfigurationManager.AppSettings.Get("portGroupCast").ToString());
        protected int PortWebCamCast = int.Parse(ConfigurationManager.AppSettings.Get("portGroupWebCam").ToString());

    }
}
