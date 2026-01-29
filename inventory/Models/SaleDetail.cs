namespace inventory.Models
{
    public class SaleDetail
    {
        public int SaleDetailId { get; set; }
        public int Sid { get; set; }  
        public int Pid { get; set; }    
        public int Quantity { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }

    }
}
