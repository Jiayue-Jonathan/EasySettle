using easySettle.ViewModel;

public class PropertyAmenitiesViewModel
{
    public int Id { get; set; }
    public List<CheckBoxItem> AmenitiesAvailable { get; set; }
    public List<int> SelectedAmenitiesIds { get; set; }
}