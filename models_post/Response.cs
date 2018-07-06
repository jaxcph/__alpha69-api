﻿using alpha69.common;

namespace models_post
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
        public int ModelId { get; set; }
    }
}
