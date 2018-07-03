
using alpha69.common;

namespace livesessions_post
{
    public class Request:RequestBase
    {

        public RequestBody Body { get; set; }

    }

    public class RequestBody
    {
        public bool IsPing { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Tags { get; set; }
        public int RequiredUserScore { get; set; }
        
    }
}

