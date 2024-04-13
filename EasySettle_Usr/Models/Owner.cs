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
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }        

        [RegularExpression(@"^\d{9}$")] // Regular expression pattern for exactly 9 digits
        public int telNo { get; set; }
        public string? Street { get; set; }
        public CityEnum City { get; set; }
        public string? ZipCode { get; set; }

        // Navigation property defined as virtual
        public virtual ICollection<Property>? Properties { get; set; }

    }

}
