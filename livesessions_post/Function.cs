using System;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace livesessions_post
{
    public class Function
    {
        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response FunctionHandler(Request input, ILambdaContext context)
        {
            var dba = new DBAccess();

            if (input.Body.IsPing)
                return new Response {StatusCode = 200, Message = dba.Test()};

            try
            {
                //validate require role
                if (!input.SourceUser.HasRole("Model"))
                    return new Response {StatusCode = 401, Message = "Access denied, requires Model role"};


                //get user
                var user = User.LoadBySourceUser(input.SourceUser, dba.Connection);
                if (user == null) //does not exist
                    return new Response {StatusCode = 404, Message = "User is not registered, hence cannot be a model"};


                //get model
                var model = Model.LoadByUser(user.Id, false, dba.Connection);
                if (model == null)
                    return new Response
                    {
                        StatusCode = 404,
                        Message = "User is not registered as a model, so cannot create a live session"
                    };


                //cleanup old sessions for model and create a new
                var ls = new LiveSession
                {
                    HostModelId = model.Id,
                    Title = input.Body.Title,
                    Description = input.Body.Description,
                    RequiredUserScore = input.Body.RequiredUserScore,
                    Tags = input.Body.Tags,
                    AllowPayPerMinute = input.Body.AllowPayPerMinute,
                    PpmProductId = input.Body.PpmProductId,
                    PpmAmount = input.Body.PpmAmount,
                    PpmMinimumJoinAmount = input.Body.PpmMinimumJoinAmount
                };
                ls.Save(dba.Connection);


                var r = new Response
                {
                    StatusCode = 200,
                    Message = "ok",
                    Body = new ResponseBody
                    {
                        HostModelId = model.Id,
                        HostUserId = user.Id,
                        LiveSessionId = ls.Id
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