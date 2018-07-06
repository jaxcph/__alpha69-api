using alpha69.common;
using alpha69.common.dto;

namespace livesessions_get_all_open
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
        public int Count { get; set; }
        public LiveSession[] LiveSessions { get; set; }
    }
}
