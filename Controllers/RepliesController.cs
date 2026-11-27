using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

namespace Natter.Controllers;

[Authorize]
public class RepliesController(ApplicationDbContext _db, UserManager<ApplicationUser> userManager)
    : Controller
{

    [HttpGet]
    public IActionResult ReplyModal(int postId)
    {
        var vm = new ReplyViewModel
        {
            parentPost = _db.Posts.Include(p => p.Author).FirstOrDefault(p => p.Id == postId),
            Replies = _db.Replies
                .Where(r => r.ParentPostId == postId)
                .Include(r => r.Author)
                .OrderBy(r => r.CreatedAt)
                .ToList()
        };
        if (vm.parentPost == null) return NotFound();
        return PartialView("_ReplyModal", vm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReply(int postId, string content)
    {
        var post = _db.Posts.Find(postId);
        if (post == null) return NotFound();
        if (string.IsNullOrWhiteSpace(content))
            return BadRequest("Reply cannot be empty.");

        var reply = new Reply
        {
            ParentPostId = postId,
            Content = content.Trim(),
            AuthorId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            CreatedAt = DateTime.UtcNow
        };
        _db.Replies.Add(reply);
        _db.SaveChanges();
        _db.Entry(reply).Reference(r => r.Author).Load();

        return PartialView("_ReplyItem", reply);
    }
}