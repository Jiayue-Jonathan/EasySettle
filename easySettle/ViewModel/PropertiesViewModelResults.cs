using easySettle.Models;

namespace easySettle.ViewModel
{
    public class PropertiesViewModelResults : BaseModel
    {
        public required string Name { get; set; }
        public required string StreetName { get; set; }
        public required string ZipCode { get; set; }
        public required int RentPrice { get; set; }
        public required int Rooms { get; set; }
        public required int BathRooms { get; set; }
        public required string Description { get; set; }
        public required string Lat { get; set; }
        public required string Lon { get; set; }
        public int? Sqft { get; set; }
        public bool PetFriendly { get; set; }
        public int AmenitiesId { get; set; }
        public int BuildingAmenitiesId { get; set; }
        public int PropertyTypeId { get; set; }
        public int CityId { get; set; }
        public string PicFile1 { get; set; }
        public string PicFile2 { get; set; }
        public string PicFile3 { get; set; }
        public string PicFile4 { get; set; }
        public string PicFile5 { get; set; }
        public List<int> SelectedAmenities { get; set; }
        public List<int> SelectedBuildingAmenities { get; set; }
    }
}