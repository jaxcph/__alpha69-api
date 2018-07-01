using alpha69.common;

namespace models_post
{
    public class Response:ResponseBase        
    {

        public int ModelId { get; set; }

        public Response()
        {
            Message = "";
            ErrorDetails = "";
        }

    }
}
