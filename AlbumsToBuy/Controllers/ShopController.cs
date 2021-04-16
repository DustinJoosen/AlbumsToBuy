using AlbumsToBuy.Repositories;
using AlbumsToBuy.Services;
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
		public ShopController(AlbumService albumService)
		{
			_albumService = albumService;
		}
		public async Task<IActionResult> Index()
		{
			var albums = await _albumService.GetAll();
			return View(albums);
		}
	}
}
