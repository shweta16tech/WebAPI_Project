namespace inventory.Models
{
    public class BaseEntity
    {
        public DateTime createdat { get; set; }
        public string createdby { get; set; }
        public DateTime? modifiedat { get; set; }
        public string? modifiedby { get; set; }
    }
}
