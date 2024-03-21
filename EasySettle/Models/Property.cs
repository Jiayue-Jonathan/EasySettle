using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasySettle.Models
{
    public class Property
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;
        public const int LongDescription = 255;

        //[Key]
        public int PropertyID { get; set; }

        //[ForeignKey("Owner")]
        public int OwnerID { get; set; }
 
        [StringLength(MaxNameLength, MinimumLength = MinNameLength)]
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public TypeEnum Type { get; set; }
        public int Rooms { get; set; }
        public int Rent { get; set; }
        public bool Rented { get; set; }



        public virtual ICollection<Photo>? Photos { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<Lease>? Leases { get; set; }
    }

    public enum TypeEnum
    {
        Apartment,
        House
    }
}
