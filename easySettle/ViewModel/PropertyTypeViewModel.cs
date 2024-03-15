using easySettle.Models;
using System.ComponentModel.DataAnnotations;

namespace easySettle.ViewModel
{
    public class PropertyTypeViewModel : BaseModel
    {
        public const int MinNameLength = 4;
        public const int MaxNameLength = 32;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string? Name { get; set; }

    }
}
