using System.ComponentModel.DataAnnotations;

namespace Natter.Models;

public class Reply : Post
{
    public int ParentPostId { get; set; }
    [Required]
    public Post ParentPost { get; set; }
}