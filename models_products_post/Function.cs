using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace models_products_post
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
            if (input.Body.IsPing)
                return new Response { StatusCode = 200, Message = "ok, ping" };

            try
            {
                var dba = new DBAccess();

                var modelProduct = new ModelProduct
                {
                    ModelId = input.Body.ModelId,
                    ProductId = input.Body.ProductId
                };
                modelProduct.Save(dba.Connection);
                

                var r = new Response()
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

            return null;
        }
    }
}