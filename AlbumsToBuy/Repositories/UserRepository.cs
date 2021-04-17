using AlbumsToBuy.Dtos;
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

		public override Task Create(User model)
		{
			model.UserToken = AlbumsToBuy.Helpers.AuthenticationHelper.CreateToken();
			return base.Create(model);
		}

		public async Task<User> GetByToken(string token)
		{
			return await _context.Users.SingleOrDefaultAsync(s => s.UserToken == token);
		}

		public async Task<User> GetByEmail(string email)
		{
			return await _context.Users.SingleOrDefaultAsync(s => s.Email == email);
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
