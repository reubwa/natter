using Microsoft.EntityFrameworkCore;

namespace Natter.Models;

[PrimaryKey(nameof(FollowerId), nameof(FolloweeId))]
public class Follow
{
    public string FollowerId { get; set; } = "";
    public ApplicationUser Follower { get; set; } = null!;

    public string FolloweeId { get; set; } = "";
    public ApplicationUser Followee { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}