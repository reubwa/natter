// Controllers/PostsController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

namespace Natter.Controllers;

[Authorize]
public class PostsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home");

        db.Posts.Add(new Post
        {
            Content = model.Content.Trim(),
            AuthorId = userManager.GetUserId(User)!
        });
        await db.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await db.Posts.FindAsync(id);
        if (post == null) return NotFound();

        if (post.AuthorId != userManager.GetUserId(User))
            return Forbid();

        db.Posts.Remove(post);
        await db.SaveChangesAsync();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Ok();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLike(int postId)
    {
        var userId = userManager.GetUserId(User);
        if (userId == null) return Challenge();

        if (!await db.Posts.AnyAsync(p => p.Id == postId)) return NotFound();

        var existing = await db.Likes.FindAsync(postId, userId);
        var liked = existing == null;

        if (liked)
            db.Likes.Add(new Like { PostId = postId, UserId = userId });
        else
            db.Likes.Remove(existing!);

        await db.SaveChangesAsync();

        var likeCount = await db.Likes.CountAsync(l => l.PostId == postId);
        return Json(new { liked, likeCount });
    }
}