using easySettle.Models;

namespace easySettle.ViewModel
{
    public class CheckBoxItem : BaseModel
    {
        public string Name { get; set; }

        public bool IsChecked { get; set; }
    }
}