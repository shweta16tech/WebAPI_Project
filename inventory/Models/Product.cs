namespace inventory.Models
{
    public class Product:BaseEntity
    {
        public int Pid { get; set; }
        public required string Pname { get; set; }
        public decimal Price { get; set; }
        public string? Descrip { get; set; }
        public int Pquantity { get; set; }
    }
}
