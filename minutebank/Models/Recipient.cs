namespace minutebank.Models
{
    public class Recipient
    {
        public int id { get; set; }
        public string name { get; set; }
        public string bank { get; set; }
        public short swift_code { get; set; }
        public string account_number { get; set; }
        public int user_id { get; set; }
    }
}
