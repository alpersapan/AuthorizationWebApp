using AuthorizationWebApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthorizationWebApp.Controllers
{
    public class SessionController : Controller
    {
        public List<User> Users { get; set; } = new List<User>();

        public SessionController()
        {
            FillUsers();
        }

        public void FillUsers()
        {
            User user1 = new()
            {
                Id = "1",
                Password = "1x2",
                Email = "alper@sapan.com",
                UserName = "alpersapan",
                FullName = "Alper Sapan",
                Role = "Administrator"
            };
            User user2 = new()
            {
                Id = "2",
                Password = "3x4",
                Email = "john@doe.com",
                UserName = "johndoe",
                FullName = "John Doe",
                Role = "Employee"
            };

            Users.Add(user1);
            Users.Add(user2);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Open()
        {
            return View();
        }

        public async Task<IActionResult> Open(User user)
        {
            User? userWhoTryingToLogin = Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();

            if (userWhoTryingToLogin != null)
            {
                var claims = new List<Claim>{
                    new Claim(ClaimTypes.Email, userWhoTryingToLogin.Email),
                    new Claim(ClaimTypes.Name, userWhoTryingToLogin.FullName),
                    new Claim(ClaimTypes.Role, userWhoTryingToLogin.Role)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties();

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Route("logout")]
        public async Task<IActionResult> Close()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}
