using easySettle.Models;

namespace easySettle.ViewModel.paginator
{
    public class PropertyTypeIndexViewModel : BaseModel
    {
        public required List<PropertyTypeViewModel> PropertyType { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }


        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
