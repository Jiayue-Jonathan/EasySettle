using easySettle.Models;

namespace easySettle.ViewModel
{
    public class RequestViewModel : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;

        public DateOnly RequestDate { get; set; }
        public RequestStatus Status { get; set; }


        public enum RequestStatus
        {
            Pending = 0,
            Requested = 1,
            Incompleted = 2,
            Declined = 3,
            Approved = 4,
            Completed = 5,
        }
    }
}
