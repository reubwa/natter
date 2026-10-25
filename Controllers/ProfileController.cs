using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

namespace Natter.Controllers;

public class ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext) : Controller
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _db = dbContext;
    
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
        
        var currentUserId = _userManager.GetUserId(User);

        vm.Posts = await _db.Posts
            .Where(p => p.AuthorId == user.Id)
            .OrderByDescending(p => p.CreatedAt)
            .Take(50)
            .Select(p => new PostViewModel
            {
                Id = p.Id,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                AuthorScreenName = p.Author.ScreenName,
                CanDelete = p.AuthorId == currentUserId
            })
            .ToListAsync();
        
        return View(vm);
    }
}