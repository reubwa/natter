namespace Natter.Models;

public class PostViewModel
{
    public int Id { get; set; }
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public string AuthorScreenName { get; set; } = "";
    public bool CanDelete { get; set; }
    public int LikeCount { get; set; }
    public bool LikedByCurrentUser { get; set; }
}