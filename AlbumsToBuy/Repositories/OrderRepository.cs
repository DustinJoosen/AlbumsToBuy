using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class OrderRepository : CrudRepository<Order>
	{
		private ApplicationDbContext _context;
		public OrderRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<List<Order>> GetAll()
		{
			return await this._context.Orders
				.Include(s => s.Payment)
				.Include(s => s.User)
				.ToListAsync();
		}

		public override async Task<Order> GetById(int id)
		{
			return await this._context.Orders
				.Include(s => s.Payment)
				.Include(s => s.User)
				.Include(s => s.Albums).ThenInclude(s => s.Album)
				.SingleOrDefaultAsync(s => s.Id == id);
		}

		public async Task<List<Order>> GetByUserId(int id)
		{
			return await this._context.Orders
				.Include(s => s.Payment)
				.Where(s => s.UserId == id)
				.ToListAsync();
		}
	}
}
