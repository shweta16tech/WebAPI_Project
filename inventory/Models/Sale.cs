namespace inventory.Models
{
    public class Sale
    {
        public int Sid { get; set; }
        public DateTime Sdate { get; set; }
        public int Squantity { get; set; }
        public int Pid { get; set; }
        public int Cid { get; set; }
        public decimal Sprice { get; set; }
        public decimal Srate { get; set; }
        public decimal Totalmnt { get; set; }
    }
}
