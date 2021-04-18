using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	public class AccountController : Controller
	{
		private UserService _userService;

		public AccountController(UserService userService)
		{
			_userService = userService;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(AuthDto auth)
		{
			if (!ModelState.IsValid)
			{
				return View(auth);
			}

			if (await _userService.CheckLogin(auth))
			{
				var user = await _userService.GetByEmail(auth.Username);

				var claims = new List<Claim>() {
					new Claim(ClaimTypes.Name, user.UserToken),
					new Claim(ClaimTypes.Role, user.Role.ToString())
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				var props = new AuthenticationProperties() { 
					ExpiresUtc = DateTime.UtcNow.AddDays(28), 
					IsPersistent = true
				};

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);
				return RedirectToAction("Index", "Home");
			}
			else
			{
				return View(auth);
			}
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(User user)
		{
			if (ModelState.IsValid)
			{
				user.Role = UserRole.Customer;
				user.UserToken = AuthenticationHelper.CreateToken();

				await _userService.Create(user);

				return RedirectToAction("Login", "Account");
			}

			return View(user);
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
