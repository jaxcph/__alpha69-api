
using alpha69.common;

namespace livesessions_goal_post
{
    public class Request:RequestBase
    {

        public RequestBody Body { get; set; }

    }

    public class RequestBody
    {
        public bool IsPing { get; set; }

        public int LiveSessionId { get; set; } //for crosschecking againt model

        public string Title { get; set; }
        public string Description  { get; set; }
        public double GoalAmount { get; set; }
        public string Tags { get; set; }

       

    }
}

