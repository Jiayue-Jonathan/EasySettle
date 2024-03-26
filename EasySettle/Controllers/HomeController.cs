using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasySettle.Models;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace EasySettle.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    // This action redirects the user to Azure AD B2C for authentication.
    public IActionResult SignIn()
    {
        var redirectUrl = Url.Action(nameof(Index), "Home");
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirectUrl },
            OpenIdConnectDefaults.AuthenticationScheme);
    }

    [Authorize] // Ensure this is only accessible for authenticated users
    public IActionResult Profile()
    {
        // Profile logic here
        // For now, we just return the view
        return View();
    }

public async Task<IActionResult> SignOut()
{
    // Build the post-logout redirect URI to redirect back to the home page after signing out from Azure AD B2C
    var postLogoutRedirectUri = Url.Action(nameof(Index), "Home", null, Request.Scheme);

    // Build the Azure AD B2C logout URL with the post-logout redirect URI
    var tenantName = "easySettle"; // Replace with your Azure AD B2C domain prefix
    var tenantId = "0eaa4f2e-a747-47b8-8f3e-fad0fa261c56"; // Replace with your Azure AD B2C tenant ID
    var logoutUrl = $"https://{tenantName}.b2clogin.com/{tenantId}/oauth2/v2.0/logout?post_logout_redirect_uri={Uri.EscapeDataString(postLogoutRedirectUri)}";
    https://localhost:7163/signin-oidc
    // Sign out locally first
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    // Redirect the user to Azure AD B2C to complete the sign-out process
    return new SignOutResult(
        new[] { OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme },
        new AuthenticationProperties { RedirectUri = logoutUrl });
}




    // public async Task<IActionResult> SignOut()
    // {
    //     // Initiates the sign-out process
    //     var callbackUrl = Url.Action(nameof(SignedOut), "Home", null, Request.Scheme);
        
    //     // Sign out of the cookie authentication
    //     await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        
    //     // Sign out of the OpenID Connect session and redirect to Azure AD B2C logout endpoint
    //     return new SignOutResult(new[] { OpenIdConnectDefaults.AuthenticationScheme, CookieAuthenticationDefaults.AuthenticationScheme },
    //                              new AuthenticationProperties { RedirectUri = callbackUrl });
    // }

    // // The SignedOut action is invoked by Azure B2C after the user has been logged out
    // public IActionResult SignedOut()
    // {
    //     // You can add any post-signout logic here, such as logging or redirecting to a specific page
    //     if (User.Identity.IsAuthenticated)
    //     {
    //         // Redirect to home page if the user is somehow still authenticated
    //         return RedirectToAction(nameof(Index));
    //     }

    //     // Redirect to the sign-in page
    //     return RedirectToAction(nameof(Index));
    // }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
