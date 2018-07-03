
using alpha69.common;

namespace livesessions_delete
{
    public class Request:RequestBase
    {

        public RequestBody Body { get; set; }

    }

    public class RequestBody
    {
        public bool IsPing { get; set; }

        public int LiveSessionId { get; set; }
        public int Rating { get; set; }
        public string Remarks { get; set; }
        
    }
}

