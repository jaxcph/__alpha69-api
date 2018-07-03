using System;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace models_post
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
                return new Response {StatusCode = 200, Message = dba.Test()};
            
            try
            {

                //validate require role
                if (!input.SourceUser.HasRole("Model"))
                    return new Response { StatusCode = 401, Message = "Access denied, requires Model role" };   


                //get or create user
                var user = User.LoadBySourceUser(input.SourceUser, dba.Connection);
                if (user == null) //does not exist
                {
                    user = new User()
                    {
                        SourceDomain = input.SourceUser.Domain,
                        SourceUserId = input.SourceUser.Id,
                        Login = input.SourceUser.Login
                    };
                    user.Save(dba.Connection);
                }

                var modelExisting = Model.LoadByUser(user.Id, dba.Connection);
                if (modelExisting != null)
                    return new Response() { StatusCode = 409, Message = "Already exists", ModelId = modelExisting.Id };

                var model = new Model
                {
                    UserId = user.Id,
                    Description = input.Body.Description,
                    Name = input.Body.Name,
                    Facebook = input.Body.Facebook,
                    Instagram = input.Body.Instagram,
                    Snapchat = input.Body.Snapchat,
                    Twitter = input.Body.Twitter,
                    Website = input.Body.Website
                };
                model.Save(dba.Connection);

          
                var r = new Response()
                {
                    StatusCode = 200,
                    Message = "ok",
                    ModelId = model.Id
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
