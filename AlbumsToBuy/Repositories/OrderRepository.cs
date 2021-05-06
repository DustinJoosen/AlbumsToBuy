using AlbumsToBuy.Dtos;
using AlbumsToBuy.Helpers;
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

		public async Task<List<Order>> GetByPage(PaginationDto pagination, bool showDelivered)
		{
			return await this._context.Orders
				.Include(s => s.Payment)
				.Include(s => s.User)
				.Where(s => (s.Status != OrderStatus.Delivered) || (showDelivered == true))
				.OrderByDescending(s => s.Id)
				.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize)
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

		public async Task<int> Count(bool showDelivered)
		{
			return await this._context.Orders
				.Where(s => (s.Status != OrderStatus.Delivered) || (showDelivered == true))
				.CountAsync();
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
