using easySettle.Models;
using System.ComponentModel.DataAnnotations;

namespace easySettle.ViewModel
{
    public class BuildingAmenitiesViewModel : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 75;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string Name { get; set; }

    }
}
