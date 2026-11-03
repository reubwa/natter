namespace Natter.Models;
public class SuggestedUserViewModel
{
    public string UserId { get; set; } = "";
    public string? ScreenName { get; set; }
    public string? ProfileImageUrl { get; set; }
    public int MutualCount { get; set; }
    public bool FollowsMe { get; set; }
}