using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace livesessions_goal_post
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
                var model = Model.LoadByUser(user.Id,true, dba.Connection);
                if (model == null)
                    return new Response() { StatusCode = 404, Message = "User is not registered as a model, so cannot create a live session" };


                var ls=LiveSession.LoadOpenForModel(model.Id, dba.Connection);
                if(ls==null)
                    return new Response() { StatusCode = 404, Message = "The model is not host of an open live session" };

                //check if model is host of session
                if (ls.Id != input.Body.LiveSessionId)
                    return new Response(){ StatusCode = 404, Message = "Inconsistance between provided live session id and one registered in the db"};

                //chekc if model is accepting product set for the goal
                Product usedProduct = null;
                foreach (var p in model.Products)
                {
                    if (p.Id == input.Body.ProductId)
                    {
                        usedProduct = p;
                        break;
                    }
                }
                if(usedProduct==null)
                    return new Response() { StatusCode = 404, Message = $"The goal product is not accepted by this model" };

                var goal=new Goal()
                {
                    LiveSessionId = ls.Id,
                    Title =input.Body.Title,
                    Description = input.Body.Description,
                    Tags = input.Body.Tags,
                    GoalAmount = input.Body.GoalAmount,
                    GoalAmountLeft = input.Body.GoalAmount,
                    ProductId = usedProduct.Id
                };
                goal.Save(dba.Connection);


                var r = new Response()
                {
                    StatusCode = 200,
                    Message = "ok",
                    Body=new ResponseBody()
                    { 
                        HostModelId = model.Id,
                        HostUserId = user.Id,
                        LiveSessionId = ls.Id,
                        ProductId = usedProduct.Id,
                        GoalId=goal.Id
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
