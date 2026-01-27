namespace inventory.Models
{
    public class Customer:BaseEntity
    {
        public int Cid { get; set; }
        public required string Cname { get; set; }
        public required string Cphone { get; set; }
        public string? Caddress { get; set; }
        public string? Cemail { get; set; }
       
    }
}
