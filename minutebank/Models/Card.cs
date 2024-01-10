namespace minutebank.Models
{
    public class Card
    {
        public int id { get; set; }
        public string? card_number { get; set; }
        public DateTime? expiry { get; set; }
        public string? cvc { get; set; }
        public int account_id { get; set; }
    }
}
