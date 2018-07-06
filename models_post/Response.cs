using alpha69.common;

namespace models_post
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
        public int ModelId { get; set; }
    }
}