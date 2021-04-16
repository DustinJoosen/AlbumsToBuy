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
			return await this._context.Payments.Include(s => s.User).ToListAsync();
		}
	}
}
