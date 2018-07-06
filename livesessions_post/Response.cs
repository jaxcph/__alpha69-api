using System.Security.AccessControl;
using alpha69.common;

namespace livesessions_post
{
    public class Response:ResponseBase        
    {

        public ResponseBody Body { get; set; }

        public Response()
        {
            Message = "";
            Details = "";
        }

    }

    public class ResponseBody
    {
        public int LiveSessionId { get; set; }
        public int HostModelId { get; set; }
        public int HostUserId { get; set; }
    }
}
