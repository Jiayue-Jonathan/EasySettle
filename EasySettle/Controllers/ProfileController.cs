using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using EasySettle.Models;
using System.Security.Claims;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using EasySettle.Data;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;




namespace EasySettle.Controllers;

public class ProfileController : Controller
{
    private readonly AzureAdB2COptions _azureAdB2COptions;

    public ProfileController(IOptions<AzureAdB2COptions> azureAdB2COptions)
    {
        _azureAdB2COptions = azureAdB2COptions.Value;
    }
    public IActionResult Index()
    {
        var model = new UserProfileViewModel
        {
            Email = User.Claims.FirstOrDefault(c => c.Type == "emails")?.Value,
            DisplayName = User.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
            ObjectId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value,
            City = User.Claims.FirstOrDefault(c => c.Type == "city")?.Value, 
            Roles = User.Claims.FirstOrDefault(c => c.Type == "extension_Roles")?.Value,
            GivenName = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")?.Value,

        };

        return View(model);
    }

}


