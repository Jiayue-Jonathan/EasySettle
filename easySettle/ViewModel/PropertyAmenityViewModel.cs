using easySettle.Models;
using System.ComponentModel.DataAnnotations;

namespace easySettle.ViewModel
{
    public class PropertyAmenityViewModel : BaseModel
    {
        public int PropertyId { get; set; }
        public int AmenityId { get; set; }

        [Required]
        public Amenities Amenities { get; set; }

        [Required]
        public required PropertiesViewModel PropertiesViewModel { get; set; }

    }
}