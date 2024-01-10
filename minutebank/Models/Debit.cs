namespace minutebank.Models
{
    public class Debit
    {
        public int? id { get; set; }
        public DateTime? date { get; set; }
        public long? amount { get; set; }

        public int? recipient_id { get; set; }
        public int account_id { get; set; }

        public string? to_account_number { get; set; }

    }
}
