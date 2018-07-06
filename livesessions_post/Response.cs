using alpha69.common;

namespace livesessions_post
{
    public class Response : ResponseBase
    {
        public Response()
        {
            Message = "";
            Details = "";
        }

        public ResponseBody Body { get; set; }
    }

    public class ResponseBody
    {
        public int LiveSessionId { get; set; }
        public int HostModelId { get; set; }
        public int HostUserId { get; set; }
    }
}