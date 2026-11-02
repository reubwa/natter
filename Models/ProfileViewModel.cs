namespace Natter.Models;

public class ProfileViewModel
{
    public string Username { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public string? ScreenName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool IsOwnProfile { get; set; }
    public List<PostViewModel> Posts { get; set; } = new();
    public string UserId { get; set; } = "";
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public bool IsFollowedByCurrentUser { get; set; }
    public bool FollowsCurrentUser { get; set; }
}