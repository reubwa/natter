using System.ComponentModel.DataAnnotations;

namespace Natter.Models;

public class Post
{
    public int Id { get; set; }

    [Required, MaxLength(500)]
    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string AuthorId { get; set; } = "";
    public ApplicationUser Author { get; set; } = null!;
    
    public ICollection<Like> Likes { get; set; } = new List<Like>();
}