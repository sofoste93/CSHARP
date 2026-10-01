using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagerApp.Controllers;

public sealed class PreferencesController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Culture(string culture, string? returnUrl)
    {
        if (culture is not ("en" or "de")) culture = "en";
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true, SameSite = SameSiteMode.Lax });
        return LocalRedirect(Url.IsLocalUrl(returnUrl) ? returnUrl! : Url.Action("Index", "Home")!);
    }
}
