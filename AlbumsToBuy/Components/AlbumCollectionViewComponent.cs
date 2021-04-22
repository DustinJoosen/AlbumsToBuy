using AlbumsToBuy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Components
{
	public class AlbumCollectionViewComponent : ViewComponent
	{

		public async Task<IViewComponentResult> InvokeAsync(IEnumerable<IAlbumCollection> albumCollections)
		{
			return View(albumCollections);
		}
	}
}
