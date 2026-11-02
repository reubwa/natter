using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

namespace Natter.Controllers;

public class FollowsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public FollowsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(string userId)
    {
        var me = _userManager.GetUserId(User);
        
        if (me == null) return Challenge();

        if (userId == me) return BadRequest();
        
        if (!await _db.Users.AnyAsync(u => u.Id == userId)) return NotFound();

        var findExistingFollow = await _db.Follows.FirstOrDefaultAsync(f => f.FollowerId == me && f.FolloweeId == userId);

        bool tf = false;
        
        if (findExistingFollow != null) _db.Follows.Remove(findExistingFollow);
        else
        {
            Follow nf = new Follow { FollowerId = me, FolloweeId = userId };
            _db.Follows.Add(nf);
            tf = true;
        }

        await _db.SaveChangesAsync();
        int followerCount = await _db.Follows.CountAsync(f => f.FolloweeId == userId);
        return Json(new { isNowFollowing = tf, followerCount });
    }
}