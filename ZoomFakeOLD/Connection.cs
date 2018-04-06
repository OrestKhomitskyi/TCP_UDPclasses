using System.Net;

namespace ZoomFake
{
    public abstract class Connection
    {
        //protected IPAddress GrouIpAddress = IPAddress.Parse(ConfigurationManager.AppSettings.Get("GroupAddress").ToString());
        protected IPAddress GrouIpAddress = IPAddress.Parse("224.5.6.7");


        public abstract void Send(byte[] Bytes);
        public abstract void Receive();
    }
}
