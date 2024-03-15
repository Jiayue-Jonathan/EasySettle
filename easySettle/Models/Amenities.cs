using System.ComponentModel.DataAnnotations;

namespace easySettle.Models
{
    public class Amenities : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 75;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string Name { get; set; }

        public required List<PropertyAmenity> PropertyAmenity { get; set; }
    }
}
