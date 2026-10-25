using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Models;

namespace Natter.Controllers;

public class ProfileController(UserManager<ApplicationUser> userManager) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

    [HttpGet("u/{screenName}")]
    public async Task<IActionResult> Index(string screenName)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.ScreenName == screenName);
        if (user == null) return NotFound();

        var vm = new ProfileViewModel
        {
            ScreenName = user.ScreenName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            Bio =  user.Bio,
            IsOwnProfile = User.Identity?.Name == user.UserName
        };
        
        return View(vm);
    }
}