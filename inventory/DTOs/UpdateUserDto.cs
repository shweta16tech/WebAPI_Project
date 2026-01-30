namespace inventory.DTOs
{
    public class UpdateUserDto
    {
        public int Uid { get; set; }
        public string? Uname { get; set; }
        public string? Uemail { get; set; }
        public string? Upass { get; set; } // Optional: only if they want to change password
    }
}