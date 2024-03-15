namespace easySettle.ViewModel
{
    public class DocumentTypeViewModel
    {
        public DocumentType Document { get; set; }
        public enum DocumentType
        {
            National = 0,
            Visa = 1,
            StudentPermit = 2,
            WorkPermit = 3,
            Passport = 4,
            Other = 9,
        }
    }
}
