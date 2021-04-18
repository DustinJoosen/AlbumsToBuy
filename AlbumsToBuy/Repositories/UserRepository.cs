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

		public async Task<bool> UniqueEmail(string email)
		{
			var result = await _context.Users.AnyAsync(s => s.Email.ToLower() == email.ToLower());
			return !result;
		}

		public async Task<User> GetByEmail(string email)
		{
			return await _context.Users
				.SingleOrDefaultAsync(s => s.Email == email);
		}

		public async Task<bool> CheckLogin(AuthDto auth)
		{
			var user = await _context.Users.SingleOrDefaultAsync(s => s.Email == auth.Username);
			if (user == null)
			{
				return false;
			}

			return user.Password == auth.Password;
		}
	}
}
