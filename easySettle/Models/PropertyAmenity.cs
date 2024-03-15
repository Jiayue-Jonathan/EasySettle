namespace easySettle.Models
{
    public class PropertyAmenity
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public virtual Properties Property { get; set; }
        public int AmenityId { get; set; }
        public virtual Amenities Amenity { get; set; }


    }
}
