namespace minutebank.Models
{
    public class Account
    {
        public int id { get; set; }
        public string? account_number { get; set; }
        public long balance { get; set; }
        public string? type { get; set; }

        public bool status { get; set; }    
        public int user_id { get; set; }
    }
}
