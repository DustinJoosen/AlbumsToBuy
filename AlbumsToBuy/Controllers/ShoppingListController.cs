﻿using AlbumsToBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	[Authorize(Roles = "Customer")]
	public class ShoppingListController : Controller
	{
		private AlbumService _albumService;
		private ShoppingListItemService _shoppingListItemService;
		private UserService _userService;
		public ShoppingListController(AlbumService albumService, ShoppingListItemService shoppingListItemService, UserService userService)
		{
			_albumService = albumService;
			_shoppingListItemService = shoppingListItemService;
			_userService = userService;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userService.GetByToken(User.Identity.Name);
			if(user == null)
			{
				return Unauthorized();
			}

			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> RemoveFromShoppingList(int Id)
		{
			var shoppingListItem = await _shoppingListItemService.GetById(Id);
			if(shoppingListItem == null)
			{
				return NotFound();
			}

			await _shoppingListItemService.Remove(shoppingListItem);
			return RedirectToAction(nameof(Index));
		}
	}
}