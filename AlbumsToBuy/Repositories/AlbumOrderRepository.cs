using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class AlbumOrderRepository : CrudRepository<AlbumOrder>
	{
		private ApplicationDbContext _context;
		public AlbumOrderRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task Create(AlbumOrder model)
		{
			var albumorder = await _context.AlbumOlders.SingleOrDefaultAsync(s => s.AlbumId == model.AlbumId && s.OrderId == model.OrderId);
			if (albumorder == null)
			{
				await base.Create(model);
				return;
			}

			albumorder.Quantity++;
			await base.Update(albumorder);
		}
	}
}
