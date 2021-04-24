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

		public override async Task<Album> GetById(int id)
		{
			return await this._context.Albums.Include(s => s.Tracks).SingleOrDefaultAsync(s => s.Id == id);
		}
		public async Task<List<AlbumOrder>> GetByOrder(Order order)
		{
			return await this._context.AlbumOlders.Include(s => s.Album).Where(s => s.OrderId == order.Id).ToListAsync();
		}

		public async Task<List<Album>> GetStocked()
		{
			return await this._context.Albums.Include(s => s.Tracks).Where(s => s.Stock > 0).ToListAsync();
		}

	}
}
