﻿using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Composition.Convention;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	public class AccountController : Controller
	{
		private UserService _userService;
		private INotyfService _notyf;

		public AccountController(UserService userService, INotyfService notyf)
		{
			_userService = userService;
			_notyf = notyf;
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
					new Claim(ClaimTypes.Name, user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role.ToString())
				};

				var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var principal = new ClaimsPrincipal(identity);
				var props = new AuthenticationProperties() { 
					ExpiresUtc = DateTime.UtcNow.AddDays(28), 
					IsPersistent = true
				};

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

				_notyf.Information($"Successfully signed in. Welcome {user.FullName}");
				return RedirectToAction("Index", "Home");
			}
			else
			{
				ModelState.AddModelError(String.Empty, "Invalid login attempt");
				return View(auth);
			}
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterDto register)
		{
			if (ModelState.IsValid)
			{
				//validation
				bool valid = true;
				if (!await _userService.UniqueEmail(register.Email))
				{
					ModelState.AddModelError("Email", "This email is already in use");
					valid = false;
				}
				if(register.Password != register.PasswordConfirmation)
				{
					ModelState.AddModelError("Password", "The passwords are not the same");
					valid = false;
				}

				if (!valid)
				{
					return View(register);
				}

				//insertion
				var user = new User()
				{
					Password = register.Password,
					Email = register.Email,
					FirstName = register.FirstName,
					LastName = register.LastName,
					Role = UserRole.UnconfirmedCustomer
				};

				await _userService.Create(user);
				MailHelper.RegistrationConfirmation(user);

				_notyf.Information($"Successfully registered your account. Welcome {user.FullName}. A confirmation email has been sent");
				return RedirectToAction("Login", "Account");
			}

			return View(register);
		}

		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> Settings()
		{
			var user = await _userService.GetById(User.Identity.Name);
			return View(user);
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Settings(User user)
		{
			if (ModelState.IsValid)
			{
				await _userService.Update(user);
				_notyf.Information($"Successfully saved your account settings.");
				return RedirectToAction("Index", "Home");
			}

			return View(user);

		}

		public async Task<IActionResult> Confirm(int? id)
		{
			if(id == null)
			{
				return BadRequest();
			}

			var user = await _userService.GetById((int)id);
			if(user == null)
			{
				return NotFound();
			}

			user.EmailConfirmed = true;
			user.Role = UserRole.Customer;
			await _userService.Update(user);

			_notyf.Information("Successfully confirmed your email. you can now order items");
			return RedirectToAction(nameof(Index), "Home");
		}

		public IActionResult AccessDenied()
		{
			_notyf.Information("You are not authorized for this action. \nplease log in with an account with more rights");
			return RedirectToAction("Login");
		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ForgotPassword(string email)
		{
			if(email == null)
			{
				return View();
			}

			var user = await _userService.GetByEmail(email);
			if(user == null)
			{
				ModelState.AddModelError(String.Empty, "this email is not registered in our system");
				return View();
			}

			MailHelper.ForgotPassword(user);

			_notyf.Information("An email has been sent, with a link to reset your password");
			return RedirectToAction(nameof(Login));
		}

		public async Task<IActionResult> ResetPassword(int id)
		{
			var user = await _userService.GetById(id);
			if(user == null)
			{
				return NotFound();
			}

			return View(id);
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(int id, string password, string passwordConfirmation)
		{
			if(password != passwordConfirmation)
			{
				ModelState.AddModelError(String.Empty, "Passwords need to be the same");
				return View(id);
			}

			var user = await _userService.GetById(id);
			if (user == null)
			{
				return NotFound();
			}

			user.Password = password;
			await _userService.Update(user);

			_notyf.Information("Your password has been updated");
			return RedirectToAction(nameof(Index), "Home");
		}
	}
}
