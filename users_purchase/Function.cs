using System;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;
using Amazon.Lambda.Serialization.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace users_purchase
{
    public class Function
    {
        private string LOGLEVEL;


        //for aws lambda invokation
        public Function()
        {
            LOGLEVEL = Environment.GetEnvironmentVariable("LOGLEVEL");
        }


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
                if (!input.SourceUser.HasRole("Member"))
                    return new Response {StatusCode = 401, Message = "Access denied, requires Model role"};


                //get or create user
                var user = User.LoadBySourceUser(input.SourceUser, dba.Connection);
                if (user == null) //does not exist
                {
                    user = new User
                    {
                        SourceDomain = input.SourceUser.Domain,
                        SourceUserId = input.SourceUser.Id,
                        Login = input.SourceUser.Login
                    };
                    user.Save(dba.Connection);
                }


                //get or create wallet
                var wallet = Wallet.LoadByUserProduxt(user.Id, input.Body.ProductId, dba.Connection);
                if (wallet == null)
                {
                    wallet = new Wallet
                    {
                        UserId = user.Id,
                        Balance = input.Body.Amount,
                        ProductId = input.Body.ProductId,
                        TotalPurchased = input.Body.Amount,
                        TotalTipped = 0
                    };
                    wallet.Save(dba.Connection);
                }
                else
                {
                    //update wallet
                    wallet.Balance += input.Body.Amount;
                    wallet.TotalPurchased += input.Body.Amount;
                    wallet.Update(dba.Connection);
                }

                //register purchase transaction
                var purchase = new Purchase(user.Id, wallet.Id)
                {
                    Amount = input.Body.Amount,
                    Note = input.Body.Note,
                    PaymentTransactionId = input.Body.PaymentTransactionId,
                    PaymentProcessor = input.Body.PaymentProcessor
                };
                purchase.Save(dba.Connection);


                var resp = new Response
                {
                    StatusCode = 200,
                    Message = "ok",
                    Body = new ResponseBody
                    {
                        UserId = user.Id,
                        WalletId = wallet.Id,
                        PurchaseId = purchase.Id,
                        ProductId = wallet.ProductId,
                        Balance = wallet.Balance
                    }
                };

                return resp;
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