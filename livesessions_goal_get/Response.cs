using System.Reflection.Metadata.Ecma335;
using alpha69.common;
using alpha69.common.dto;

namespace livesessions_goals_get
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
        public Goal[] Goals { get; set; }
    }
}
