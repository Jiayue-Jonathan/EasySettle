namespace easySettle.ViewModel.paginator
{
    public class PropertyIndexResultViewModel
    {
        public required List<PropertiesViewModelResults> Properties { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
