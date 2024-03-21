using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySettle.Models
{
    public class Owner
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;
        public const int LongDescription = 255;


        public int OwnerID { get; set; }

        [StringLength(MaxNameLength, MinimumLength = MinNameLength)] // Setting min and max length for street
        public string? Street { get; set; }

        public string? City { get; set; }
        public string? ZipCode { get; set; }

        

        [RegularExpression(@"^\d{9}$")] // Regular expression pattern for exactly 9 digits
        public int telNo { get; set; }

        // Navigation property defined as virtual
        public virtual ICollection<Property>? Properties { get; set; }

    }

}
