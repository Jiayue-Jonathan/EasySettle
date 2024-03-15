using easySettle.Models;
using System.ComponentModel.DataAnnotations;

namespace easySettle.ViewModel
{
    public class CityViewModel : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string? CityName { get; set; }


    }
}

