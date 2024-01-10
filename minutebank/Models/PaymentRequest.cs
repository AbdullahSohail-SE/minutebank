namespace minutebank.Models
{
    public class PaymentRequest
    {
        public int id { get; set; }
        public string? sent_to { get; set; }
        public long amount { get; set; }

        public DateTime? due_by { get; set; }
        public bool status { get; set; }
        public string account_number { get; set; }
        public int user_id { get; set; }
    }
}
