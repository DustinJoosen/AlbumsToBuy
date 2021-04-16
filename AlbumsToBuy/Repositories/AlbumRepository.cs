using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class AlbumRepository : CrudRepository<Album>
	{
		private ApplicationDbContext _context;
		public AlbumRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<List<Album>> GetAll()
		{
			return await this._context.Albums.Include(s => s.Tracks).ToListAsync();
		}
	}
}
