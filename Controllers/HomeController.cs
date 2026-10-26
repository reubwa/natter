using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Natter.Data;
using Natter.Models;

public class HomeController(ApplicationDbContext db) : Controller
{
    public async Task<IActionResult> Index(Enums.FeedSort sort = Enums.FeedSort.Newest)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        IQueryable<Post> query = db.Posts;

        switch (sort)
        {
            case Enums.FeedSort.Newest:
                query = query.OrderByDescending(p => p.CreatedAt);
                break;
            case Enums.FeedSort.Oldest:
                query = query.OrderBy(p => p.CreatedAt);
                break;
            case Enums.FeedSort.MostLiked:
                query = query.OrderByDescending(p => p.Likes.Count).ThenByDescending(p => p.CreatedAt);
                break;
            case Enums.FeedSort.Following:
                throw new NotImplementedException();
            case Enums.FeedSort.FollowedBy:
                throw new NotImplementedException();
            case Enums.FeedSort.Friends:
                throw new NotImplementedException();
            default:
                query = query.OrderByDescending(p => p.CreatedAt);
                break;
        }

        var posts = await query
            .Take(50)
            .Select(p => new PostViewModel
            {
                Id = p.Id,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                AuthorScreenName = p.Author.ScreenName,
                CanDelete = p.AuthorId == currentUserId,
                LikeCount =  p.Likes.Count,
                LikedByCurrentUser = p.Likes.Any(l => l.UserId == currentUserId)
            })
            .ToListAsync();

        return View(new HomeViewModel { Sort = sort, Posts = posts });
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