using System.ComponentModel.DataAnnotations;

namespace Natter.Models;

public class CreatePostViewModel
{
    [Required, StringLength(500, MinimumLength = 1)]
    public string Content { get; set; } = "";
}