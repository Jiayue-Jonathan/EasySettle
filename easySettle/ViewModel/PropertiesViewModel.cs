using easySettle.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace easySettle.ViewModel
{
    public class PropertiesViewModel : BaseModel
    {
        public const int MinNameLength = 2;
        public const int MaxNameLength = 32;
        public const int LongDescription = 255;

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        public string? Name { get; set; }

        [Required]
        [MinLength(MinNameLength), MaxLength(MaxNameLength)]
        [Display(Name = "Street")]
        public string? StreetName { get; set; }

        [Required]
        public string? ZipCode { get; set; }
        [Required]
        [Display(Name = "Rent Price $")]
        public int RentPrice { get; set; }

        [Required]
        [Display(Name = "Rooms")]
        public int Rooms { get; set; }

        [Required]
        [Display(Name = "Bath Rooms")]
        public int BathRooms { get; set; }

        [Required]
        [MinLength(MinNameLength), MaxLength(LongDescription)]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Latitude")]
        public string? Lat { get; set; }
        [Required]
        [Display(Name = "Longitude")]

        public string? Lon { get; set; }
        [Display(Name = "Squared Feet")]

        public int? Sqft { get; set; }
        [Display(Name = "Pet Friendly")]
        public bool PetFriendly { get; set; }
        [Required]
        [ForeignKey("AmenitiesId")]
        [Display(Name = "Amenities")]

        public int AmenitiesId { get; set; }

        [Required]
        [ForeignKey("BuildingAmenitiesId")]
        [Display(Name = "Building Amenities")]
        public int BuildingAmenitiesId { get; set; }

        [Required]
        [ForeignKey("PropertyTypeId")]
        [Display(Name = "Property Type")]
        public int PropertyTypeId { get; set; }

        [Required]
        [Display(Name = "City")]
        [ForeignKey("CityId")]
        public int CityId { get; set; }

        public IFormFile PicFile1 { get; set; }
        public IFormFile PicFile2 { get; set; }
        public IFormFile PicFile3 { get; set; }
        public IFormFile PicFile4 { get; set; }
        public IFormFile PicFile5 { get; set; }
        public List<int> SelectedAmenities { get; set; }
        public List<int> SelectedBuildingAmenities { get; set; }
    }
}