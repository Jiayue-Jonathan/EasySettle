using System.ComponentModel.DataAnnotations;

namespace easySettle.Models
{
    public class PropertyType : BaseModel
    {
        public const int MinNameLength = 4;
        public const int MaxNameLength = 32;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string? Name { get; set; }

    }
}
