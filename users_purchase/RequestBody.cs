namespace users_purchase
{
    public class RequestBody
    {
        public bool IsPing { get; set; }

        public int ProductId { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PaymentProcessor { get; set; }
    }
}