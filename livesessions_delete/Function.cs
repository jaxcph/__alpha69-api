using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace livesessions_delete
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


                //validate require role
                if (!input.SourceUser.HasRole("Model"))
                    return new Response { StatusCode = 401, Message = "Access denied, requires Model role" };


                //get user
                var user = User.LoadBySourceUser(input.SourceUser, dba.Connection);
                if (user == null) //does not exist
                    return new Response() { StatusCode = 404, Message = "User is not registered, hence cannot be a model" };


                //get model
                var model = Model.LoadByUser(user.Id, dba.Connection);
                if (model == null)
                    return new Response() { StatusCode = 404, Message = "User is not registered as a model, so cannot create a live session" };


                //validate that we received the correct live session id.
                //the calling model must also be the host of the livesession and the live session must not be closed/have ended
                var lsModel = LiveSession.LoadOpenForModel(model.Id, dba.Connection);
                if (lsModel == null)
                    return new Response { StatusCode = 400, Message = "the calling model does not have any open live session" };

                var lsReq = LiveSession.LoadOpenById(input.Body.LiveSessionId, dba.Connection);
                if (lsReq == null)
                    return new Response { StatusCode = 400, Message = "the requested live session does not exists or is already closed"};

                if (lsModel.Id != lsReq.Id)
                    return new Response { StatusCode = 400, Message = "invalid live session id" };

                lsReq.EndedRating = input.Body.Rating;
                lsReq.EndedRemark = input.Body.Remarks;
                lsReq.EndSession(dba.Connection);

               
                var r = new Response()
                {
                    StatusCode = 200,
                    Message = "ok",
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
