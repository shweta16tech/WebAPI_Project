namespace inventory.Models
{
    public class Sale:BaseEntity
    {
        public int Sid { get; set; }
        public DateTime Sdate { get; set; }
        public int Cid { get; set; }
        public decimal Totalmnt { get; set; }
        public DateTime? invoicedate { get; set; }
        public List<SaleDetail> SaleDetails { get; set; }
    }
}
