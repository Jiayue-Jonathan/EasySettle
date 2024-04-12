namespace EasySettle.Models;

public class SearchCriteria
{
    public decimal? MinRooms { get; set; }
    public decimal? MaxRooms { get; set; }

    public decimal? MinBathRooms { get; set; }
    public decimal? MaxBathRooms { get; set; }

    public decimal? MinRent { get; set; }
    public decimal? MaxRent { get; set; }

    public TypeEnum? Type { get; set; }

    public CityEnum? City { get; set; }

    public bool? Parking { get; set; }

    public bool? Pets { get; set; }

}
