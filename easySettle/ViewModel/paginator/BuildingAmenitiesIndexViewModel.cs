using easySettle.Models;

namespace easySettle.ViewModel.paginator
{
    public class BuildingAmenitiesIndexViewModel : BaseModel
    {
        public required List<BuildingAmenitiesViewModel> BuildingAmenities { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }


    }
}
