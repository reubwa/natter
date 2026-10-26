// Controllers/PostsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

[Authorize]
public class PostsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePostViewModel model)
    {
        if (!ModelState.IsValid)
            return RedirectToAction("Index", "Home");

        _db.Posts.Add(new Post
        {
            Content = model.Content.Trim(),
            AuthorId = _userManager.GetUserId(User)!
        });
        await _db.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var post = await _db.Posts.FindAsync(id);
        if (post == null) return NotFound();

        if (post.AuthorId != _userManager.GetUserId(User))
            return Forbid();

        _db.Posts.Remove(post);
        await _db.SaveChangesAsync();

        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            return Ok();

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleLike(int postId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Challenge();
        
        var post = await _db.Posts.FindAsync(postId);
        if (post == null) return NotFound();
        bool tf = false;
        var dupCheck = await _db.Likes.FindAsync(postId, userId);
        if (dupCheck == null)
        {
            _db.Likes.Add(new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Post = post,
                User = _db.Users.Find(userId)!
            });
            tf = true;
        }
        else
        {
            _db.Likes.Remove(dupCheck);
            tf = false;
        }
        await  _db.SaveChangesAsync();
        var count = post.Likes.Count;
        return Json(new { liked = tf, likeCount = count });
    }
}