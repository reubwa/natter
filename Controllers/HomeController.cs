using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

public class HomeController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var posts = await db.Posts
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

        return View(posts);
    }

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