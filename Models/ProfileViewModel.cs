namespace Natter.Models;

public class ProfileViewModel
{
    public string Username { get; set; } = "";
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Bio { get; set; }
    public string? ScreenName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public bool IsOwnProfile { get; set; }
}