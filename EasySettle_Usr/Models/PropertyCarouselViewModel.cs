// PropertyCarouselViewModel.cs
namespace EasySettle.Models
{
    public class PropertyCarouselViewModel
    {
        public IEnumerable<PropertyViewModel> Properties { get; set; }
        public string FormSubmitController { get; set; }
        public string FormSubmitAction { get; set; }
    }
}
