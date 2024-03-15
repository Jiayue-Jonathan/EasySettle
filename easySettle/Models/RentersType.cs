using System.ComponentModel.DataAnnotations;

namespace easySettle.Models
{
    public class RentersType : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string? Name { get; set; }

    }
}
