using System;
using alpha69.common;
using alpha69.common.dto;
using Amazon.Lambda.Core;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace users_purchase
{
    public class Function
    {


        private string LOGLEVEL;
        

        //for aws lambda invokation
        public Function()
        {
            this.LOGLEVEL = System.Environment.GetEnvironmentVariable("LOGLEVEL");
        }

       
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public Response FunctionHandler(Request request, ILambdaContext context)
        {

            if (request.Body.IsPing)
                return new Response { StatusCode = 200,Message="ok, ping" };
          
            try
            {

              var dba = new DBAccess();

                //get or create user
                var user=User.LoadBySourceUser(request.SourceUser,dba.Connection);
                if (user == null) //does not exist
                {
                    user = new User()
                    {
                        SourceDomain = request.SourceUser.Domain,
                        SourceUserId = request.SourceUser.Id,
                        Login = request.SourceUser.Login
                    };
                    user.Save(dba.Connection);
                }

         
                //get or create wallet
                var wallet = Wallet.LoadByUserProduxt(user.Id,request.Body.ProductId, dba.Connection);
                if (wallet == null)
                {
                    wallet = new Wallet()
                    {
                        UserId =user.Id,
                        Balance = request.Body.Amount,
                        ProductId = request.Body.ProductId,
                        TotalPurchased = request.Body.Amount,
                        TotalTipped = 0
                    };
                    wallet.Save(dba.Connection);
                }
                else
                {
                    //update wallet
                    wallet.Balance+=request.Body.Amount;
                    wallet.TotalPurchased += request.Body.Amount;
                    wallet.Update(dba.Connection);
                }

                //register purchase transaction
                var purchase = new Purchase(user.Id, wallet.Id)
                {
                    Amount = request.Body.Amount,
                    Note = request.Body.Note,
                    PaymentTransactionId = request.Body.PaymentTransactionId,
                    PaymentProcessor = request.Body.PaymentProcessor
                };
                purchase.Save(dba.Connection);


                var resp = new Response
                {
                    StatusCode=200,
                    Message="ok",
                    UserId = user.Id,
                    WalletId = wallet.Id,
                    PurchaseId = purchase.Id,
                    ProductId=wallet.ProductId,
                    Balance=wallet.Balance
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
