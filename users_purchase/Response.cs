
using alpha69.common;


namespace users_purchase
{
    public class Response:ResponseBase
    {

        public ResponseBody Body { get; set; }
        public Response()
        {
            Message = "";
            Details = "";
        }

    }

    public class ResponseBody
    {
        public int UserId { get; set; }
        public int WalletId { get; set; }
        public int ProductId { get; set; }
        public int PurchaseId { get; set; }
        public double Balance { get; set; }

    }
}
