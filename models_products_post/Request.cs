
using alpha69.common;

namespace models_products_post
{
    public class Request:RequestBase
    {

        public RequestBody Body { get; set; }

    }

    public class RequestBody
    {
        public bool IsPing { get; set; }

        public int ModelId { get; set; }
        public int ProductId { get; set; }

    }
}

