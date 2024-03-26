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
        public string? Street { get; set; }
        public CityEnum City { get; set; }
        public string? ZipCode { get; set; }
        public TypeEnum Type { get; set; }
        public decimal Rooms { get; set; }
        public decimal BathRooms { get; set; }
        public int Rent { get; set; }
        public bool Rented { get; set; }
        public bool Parking { get; set; }
        public bool Pets { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual ICollection<Lease>? Leases { get; set; }
    }

    public enum TypeEnum
    {
        Apartment,
        House
    }

    public enum CityEnum
    {
        Vancouver,
        Burnaby,
        Richmond,
        Surrey,
        Langley,
        Coquitlam,
        NorthVancouver,
        WestVancouver,
        NewWestminster,
        Delta,
        MapleRidge,
        // Add other cities as needed
    }

}
