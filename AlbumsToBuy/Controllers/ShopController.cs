using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
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
			var albums = await _albumService.GetStocked();
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
		[Authorize(Roles = "Admin, Customer")]
		public async Task<IActionResult> AddToShoppingCart(int Id)
		 {
			//get the album that will be added to the shoppingcart
			var album = await _albumService.GetById(Id);
			if(album == null || album.Stock <= 0)
			{
				return NotFound();
			}

			//get the currently logged in user
			var user = await _userService.GetById(Convert.ToInt32(User.Identity.Name));
			if (user == null)
			{
				return Unauthorized();
			}

			//check if this album already is in the shoppingList
			var shoppingListItem = user.ShoppingListItems.SingleOrDefault(s => s.Album.Name == album.Name);
			if (shoppingListItem == null)
			{
				//if the album is not in the shoppingList, add it
				await _shoppingListItemService.Create(new ShoppingListItem()
				{
					UserId = user.Id,
					AlbumId = album.Id,
					Quantity = 1
				});
			}
			//if the album is already in the shoppingList, check if the quantity is more then the stock. if no, increment the quantity 
			else if (shoppingListItem.Quantity < shoppingListItem.Album.Stock)
			{
				shoppingListItem.Quantity++;
				await _shoppingListItemService.Update(shoppingListItem);
			}
			//when there is not enough stock to increment the quantity
			else
			{
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> SearchFor(ShopSearchDto search)
		{
			if (String.IsNullOrWhiteSpace(search.SearchValue))
			{
				return RedirectToAction(nameof(Index));
			}

			var albums = await _albumService.Search(search);
			
			ViewData["SearchType"] = Convert.ToInt32(search.SearchType);
			ViewData["SearchValue"] = search.SearchValue;

			return View("Index", albums);
		}
	}
}
