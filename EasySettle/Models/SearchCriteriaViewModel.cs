namespace EasySettle.Models;

public class SearchCriteria
{
    public bool IsRoomsSearchActive { get; set; }
    public decimal? MinRooms { get; set; }
    public decimal? MaxRooms { get; set; }

    public bool IsBathRoomsSearchActive { get; set; }
    public decimal? MinBathRooms { get; set; }
    public decimal? MaxBathRooms { get; set; }

    public bool IsRentSearchActive { get; set; }
    public decimal? MinRent { get; set; }
    public decimal? MaxRent { get; set; }

    // Add other properties as needed
}