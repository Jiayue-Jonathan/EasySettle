using Microsoft.AspNetCore.Identity;

namespace easySettle.ViewModel
{
    public class ExternalUsersViewModel : IdentityUser
    {
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public DateTime DOB { get; set; }
        public byte[]? ProPhoto { get; set; }
    }
}
