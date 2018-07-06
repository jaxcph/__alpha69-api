using alpha69.common;

namespace livesessions_get_all_open
{
    public class Request : RequestBase
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public bool IsPing { get; set; }
    }
}