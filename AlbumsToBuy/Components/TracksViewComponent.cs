using AlbumsToBuy.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Components
{
	public class TracksViewComponent : ViewComponent
	{
		public async Task<IViewComponentResult> InvokeAsync(List<Track> tracks)
		{

			return View(tracks);
		}
	}
}
