namespace inventory.DTOs
{
    public class CreateUserDto
    {
        public required int Uid { get; set; }
        public required string Uname { get; set; }
        public required string Upass { get; set; }
    }
}
