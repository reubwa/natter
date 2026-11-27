namespace Natter.Models;

public class ReplyViewModel : PostViewModel
{
    public Post parentPost { get; set; }
}