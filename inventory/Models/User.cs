namespace inventory.Models
{
    public class User:BaseEntity
    {
        public int Uid { get; set; }
        public required string Uname { get; set; }
        public string? Uemail { get; set; }
        public string UpassH { get; set; }
      
    }
}
