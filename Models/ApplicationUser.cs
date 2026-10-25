using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Natter.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [MaxLength(100)]
    public string? FirstName { get; set; }
    [PersonalData]
    [MaxLength(100)]
    public string? LastName { get; set; }
    [PersonalData]
    public DateOnly? BirthDate { get; set; }
    [MaxLength(100)]
    public string? ScreenName { get; set; }
    [MaxLength(500)]
    public string? Bio { get; set; }
}