using Microsoft.AspNetCore.Identity;

namespace AuthorizationWebApp.Models
{
    public class User : IdentityUser
    {
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
