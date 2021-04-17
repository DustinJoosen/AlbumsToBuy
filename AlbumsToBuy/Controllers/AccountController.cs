using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
using AlbumsToBuy.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
				Response.Cookies.Append("token", user.UserToken);

				return RedirectToAction(nameof(Login));
			}
			else
			{
				return View(auth);
			}
		}
	}
}
