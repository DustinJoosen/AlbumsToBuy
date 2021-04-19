using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using AlbumsToBuy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Controllers
{
	public class ShopController : Controller
	{
		private AlbumService _albumService;
		private ShoppingListItemService _shoppingListItemService;
		private UserService _userService;
		public ShopController(AlbumService albumService, ShoppingListItemService shoppingListItemService, UserService userService)
		{
			_albumService = albumService;
			_shoppingListItemService = shoppingListItemService;
			_userService = userService;
		}
		public async Task<IActionResult> Index()
		{
			var albums = await _albumService.GetAll();
			return View(albums);
		}

		public async Task<IActionResult> Album(int id)
		{
			var album = await _albumService.GetById(id);
			if(album == null)
			{
				return NotFound();
			}

			return View(album);
		}

		[HttpPost]
		[Authorize(Roles ="Customer")]
		public async Task<IActionResult> AddToShoppingCart(int Id)
		{
			var album = await _albumService.GetById(Id);
			if(album == null)
			{
				return NotFound();
			}

			var user = await _userService.GetById(Convert.ToInt32(User.Identity.Name));
			if (user == null || user.Role != UserRole.Customer)
			{
				return Unauthorized();
			}

			await _shoppingListItemService.Create(new ShoppingListItem()
			{
				UserId = user.Id,
				AlbumId = album.Id,
				Quantity = 1
			});
			
			return RedirectToAction(nameof(Index));
		}
	}
}
