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
    public string ProfileImageUrl { get; set; } = "";
    public ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public int ReplyCount { get; set; }
}