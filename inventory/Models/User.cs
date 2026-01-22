namespace inventory.Models
{
    public class User
    {
        public int Uid { get; set; }
        public required string Uname { get; set; }
        public string? Uemail { get; set; }
        public required string Upass { get; set; }
    }
}
