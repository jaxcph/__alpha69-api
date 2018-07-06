using System;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace models_products_delete
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


                var modelProduct = new ModelProduct
                {
                    ModelId = input.Body.ModelId,
                    ProductId = input.Body.ProductId
                };
                modelProduct.Delete(dba.Connection);


                var r = new Response
                {
                    StatusCode = 200,
                    Message = "ok"
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