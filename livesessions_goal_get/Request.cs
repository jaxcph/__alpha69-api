using alpha69.common;

namespace livesessions_goals_get
{
    public class Request : RequestBase
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public bool IsPing { get; set; }

        public int LiveSessionId { get; set; }
    }
}