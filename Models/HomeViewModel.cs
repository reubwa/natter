namespace Natter.Models;

public class HomeViewModel
{
    public Enums.FeedSort Sort { get; set; } = Enums.FeedSort.Newest;
    public List<PostViewModel> Posts { get; set; } = new();
}