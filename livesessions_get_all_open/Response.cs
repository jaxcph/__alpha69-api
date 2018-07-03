using alpha69.common;
using alpha69.common.dto;

namespace livesessions_get_all_open
{
    public class Response:ResponseBase        
    {

        public int Count { get; set; }
        public LiveSession[] Items { get; set; }

        public Response()
        {
            Message = "";
            ErrorDetails = "";
        }

    }
}
