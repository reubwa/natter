using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Natter.Models;

[PrimaryKey(nameof(PostId), nameof(UserId))]
public class Like
{
    public int PostId { get; set; }
    public Post Post { get; set; } = null!;

    public string UserId { get; set; } = "";
    public ApplicationUser User { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}