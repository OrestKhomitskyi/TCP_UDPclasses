using System.Net;

namespace ZoomFake_TCP_
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
