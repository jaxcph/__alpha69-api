using alpha69.common;

namespace livesessions_post
{
    public class Response:ResponseBase        
    {

        public int LiveSessionId { get; set; }
        public int HostModelId { get; set; }
        public int HostUserId { get; set; }


        public Response()
        {
            Message = "";
            ErrorDetails = "";
        }

    }
}
