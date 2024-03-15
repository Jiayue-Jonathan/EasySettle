namespace easySettle.Models
{
    public class FullAudit
    {
        public string? UpdateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
