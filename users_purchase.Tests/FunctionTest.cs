using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using users_purchase;
using users_purchase.messages;
using users_purchase.models;
using users_purchase.messages.models;

namespace users_purchase.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {

            // Invoke the lambda function and confirm the string was upper cased.
            var function = new Function("alpha69.cfuzrlbo4jgs.eu-central-1.rds.amazonaws.com", "dbadmin", "fPA4Y9iZK15Tn1kw", "alpha69", 3306, "required");
            var context = new TestLambdaContext();

            var request=new Request();
            request.SourceUser = new SourceUser
            {
                Id = "1",
                Domain = "joeykim.tv",
                Login = "jax",
                Roles = "administrator,subscriber",
                IPAddress="127.0.0.1"
            };


            request.Amount = 1000;
            request.Note = "initial purchase";
            request.ProductId = 100;
            request.PaymentProcessor = "authorize.net";
            request.PaymentTransactionId = "A508804154";
            


            var response = function.FunctionHandler(request, context);

            Assert.Equal(200, response.StatusCode);
        }
    }
}
