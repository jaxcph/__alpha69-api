using alpha69.common;

namespace models_post
{
    public class Request : RequestBase
    {
        public RequestBody Body { get; set; }
    }

    public class RequestBody
    {
        public bool IsPing { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string Snapchat { get; set; }
    }
}