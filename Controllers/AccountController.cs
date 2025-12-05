using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

//Should require authorization for account, while allowing the user to still access login page
public class AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr) : Controller
{
    private readonly UserManager<AppUser> _userManager = userMgr;
    private readonly SignInManager<AppUser> _signInManager = signInMgr;

    [AllowAnonymous]
    public IActionResult Login(string returnUrl)
    {
        // return url remembers the user's original request
        ViewBag.returnUrl = returnUrl;
        return View();
    }

    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLogin details, string returnUrl)
    {
        if (ModelState.IsValid)
        {
            AppUser user = await _userManager.FindByEmailAsync(details.Email);
            if (user != null)
            {
                await _signInManager.SignOutAsync();
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, details.Password, false, false);
                if (result.Succeeded)
                {
                    return Redirect(returnUrl ?? "/");
                }
            }
            ModelState.AddModelError(nameof(UserLogin.Email), "Invalid user or password");
        }
        return View(details);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public ViewResult AccessDenied() => View();
}