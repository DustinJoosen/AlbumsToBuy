using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class UserRepository : CrudRepository<User>
	{
		private ApplicationDbContext _context;
		public UserRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public override async Task<User> GetById(int id)
		{
			return await this._context.Users
				.Include(s => s.ShoppingListItems).ThenInclude(s => s.Album)
				.SingleOrDefaultAsync(s => s.Id == id);
		}
	}
}
