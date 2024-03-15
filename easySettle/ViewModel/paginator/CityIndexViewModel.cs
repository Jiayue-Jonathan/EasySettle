using easySettle.Models;

namespace easySettle.ViewModel.paginator
{
    public class CityIndexViewModel : BaseModel
    {
        public required List<CityViewModel> City { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }

        public string CityName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
