namespace minutebank.Models
{
    public class AccountStats
    {
        public string type { get; set; }
        public DateTime date { get; set; }
        public long amount { get; set; }
        public string account { get; set; }
        public string name { get; set; }

        public string counterparty_account { get; set; }
    }
}
