namespace easySettle.Models
{
    public class PropertyBuildingAmenity
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public virtual Properties Property { get; set; }
        public int BuildingAmenitiesId { get; set; }
        public virtual BuildingAmenities BuildingAmenity { get; set; }

    }
}
