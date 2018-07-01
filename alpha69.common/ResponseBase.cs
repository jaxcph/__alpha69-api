
using System;

namespace alpha69.common
{
    public class ResponseBase
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorDetails { get; set; }

 
        public ResponseBase()
        {

        }

        public void Set(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }


        public void SetError(int statusCode, Exception e)
        {
            StatusCode = statusCode;
            Message = e.Message;
            ErrorDetails = e.ToString();
     
        }


    }
}
