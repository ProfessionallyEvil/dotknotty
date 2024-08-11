using Microsoft.AspNetCore.Identity;

namespace DotKnotty.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Role { get; set; }
    }
}