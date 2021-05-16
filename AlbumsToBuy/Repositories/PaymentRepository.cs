using AlbumsToBuy.Dtos;
using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class PaymentRepository : CrudRepository<Payment>
	{
		private ApplicationDbContext _context;
		public PaymentRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<List<Payment>> GetAll()
		{
			return await this._context.Payments
				.Include(s => s.User)
				.ToListAsync();
		}

		public async Task<int> Count(bool showPaid)
		{
			return await this._context.Payments
				.Where(s => (s.Status != PaymentStatus.Payed) || (showPaid == true))
				.CountAsync();
		}

		public async Task<List<Payment>> GetUnpaid()
		{
			return await this._context.Payments
				.Include(s => s.User)
				.Where(s => s.Status != PaymentStatus.Payed)
				.ToListAsync();
		}

		public async Task<List<Payment>> GetByPage(PaginationDto pagination, bool showPaid)
		{
			return await this._context.Payments
				.Include(s => s.User)
				.Where(s => (s.Status != PaymentStatus.Payed) || (showPaid == true))
				.OrderByDescending(s => s.Id)
				.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize)
				.ToListAsync();
		}
	}
}
