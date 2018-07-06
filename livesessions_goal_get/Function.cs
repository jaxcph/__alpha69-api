using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace livesessions_goals_get
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response FunctionHandler(Request input, ILambdaContext context)
        {
            var dba = new DBAccess();

            if (input.Body.IsPing)
                return new Response { StatusCode = 200, Message = dba.Test() };

            try
            {

                //get user
                var user = User.LoadBySourceUser(input.SourceUser, dba.Connection);
                if (user == null) //does not exist
                    return new Response() { StatusCode = 404, Message = "User is not registered" };
                
                 
                var list=Goal.LoadOpenByLiveSession(input.Body.LiveSessionId,dba.Connection);
                

                var r = new Response()
                {
                    StatusCode = 200,
                    Message = "ok",
                    Body = new ResponseBody()
                    {
                        Count = list.Count,
                        Goals = list.ToArray()
                    }
                };

                return r;


            }
            catch (Exception e)
            {
                var r = new Response();
                r.SetError(400, e);
                return r;
            }
        }
    }
}
