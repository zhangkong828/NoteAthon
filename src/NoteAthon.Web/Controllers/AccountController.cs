using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NoteAthon.Web.Infrastructure;
using NoteAthon.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NoteAthon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserConfig _userConfig;
        public AccountController(IOptionsSnapshot<UserConfig> userConfigAccessor)
        {
            _userConfig = userConfigAccessor.Value;
        }

        public IActionResult Login(string returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(returnUrl)) returnUrl = "/";
            ViewData["ReturnUrl"] = FormatHelper.UrlDecode(returnUrl);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Json(new { code = -1, msg = "参数错误" });
            }

            var user = _userConfig.Users.Where(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (user == null) return Json(new { code = -1, msg = "用户不存在" });
            if (user.Password != password) return Json(new { code = -1, msg = "密码错误" });

            var claims = new List<Claim>
            {
                new Claim("UserId", user.Id),
                new Claim("UserName", user.UserName)
            };

            var expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(7));
            AuthenticationProperties props = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = expires
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            return Json(new { code = 0, msg = "登录成功" });
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Account/Login");
        }
    }
}
