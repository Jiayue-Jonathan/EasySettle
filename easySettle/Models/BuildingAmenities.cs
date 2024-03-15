using System.ComponentModel.DataAnnotations;

namespace easySettle.Models
{
    public class BuildingAmenities : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 75;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string Name { get; set; }

        public List<PropertyBuildingAmenity> PropertyBuildingAmenity { get; set; }
    }
}
