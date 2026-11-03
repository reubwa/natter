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
        
        var followingIds = db.Follows
            .Where(f => f.FollowerId == currentUserId)
            .Select(f => f.FolloweeId);

        var followerIds = db.Follows
            .Where(f => f.FolloweeId == currentUserId)
            .Select(f => f.FollowerId);
        
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
                query = query.Where(p => followingIds.Contains(p.AuthorId))
                    .OrderByDescending(p => p.CreatedAt);
                break;
            case Enums.FeedSort.FollowedBy:
                query = query.Where(p => followerIds.Contains(p.AuthorId))
                    .OrderByDescending(p => p.CreatedAt);
                break;
            case Enums.FeedSort.Friends:
                query = query.Where(p => followingIds.Contains(p.AuthorId)
                                         && followerIds.Contains(p.AuthorId))
                    .OrderByDescending(p => p.CreatedAt);
                break;
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
                LikedByCurrentUser = p.Likes.Any(l => l.UserId == currentUserId),
                ProfileImageUrl = p.Author.ProfileImageUrl
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