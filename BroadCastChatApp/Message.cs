using System.Net;

namespace BroadCastChatApp
{
    public class Message
    {
        //Capturing a number of people names
        public IPAddress Address { get; set; }
        public string Data { get; set; }


        public override string ToString()
        {
            return $"{Data} {Address}";
        }

    }
}
