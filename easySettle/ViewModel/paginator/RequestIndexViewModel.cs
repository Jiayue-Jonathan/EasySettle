namespace easySettle.ViewModel.paginator
{
    public class RequestIndexViewModel
    {
        public required List<RequestViewModel> Request { get; set; }
        public required PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
