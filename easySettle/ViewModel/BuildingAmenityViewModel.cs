namespace easySettle.ViewModel
{
    public class BuildingAmenityViewModel : BaseModel
    {
        public required string Name { get; set; }

        public required List<PropertyBuildingAmenity> PropertyBuildingAmenities { get; set; }
    }
}
