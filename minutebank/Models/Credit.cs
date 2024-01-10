namespace minutebank.Models
{
    public class Credit
    {
        public int? id { get; set; }
        public DateTime? date { get; set; }
        public long? amount { get; set; }

        public string? from_account_number { get; set; }
        public int? payment_request_id { get; set; }
        public int account_id { get; set; }
    }
}
