namespace EasySettle.Models;

public class UserProfileViewModel
{
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public string? ObjectId { get; set; }
    public string? City { get; set; } // New
    public string? GivenName { get; set; } // New
    public string? Roles { get; set; } // New
}
