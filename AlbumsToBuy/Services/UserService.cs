using AlbumsToBuy.Dtos;
using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class UserService : CrudService<User>
	{
		private UserRepository repos;
		public UserService(UserRepository repos) : base(repos)
		{
			this.repos = repos;
		}

		public async Task<bool> CheckLogin(AuthDto auth)
		{
			return await this.repos.CheckLogin(auth);
		}

		public async Task<User> GetByEmail(string username)
		{
			return await this.repos.GetByEmail(username);
		}		
		
		public async Task<bool> UniqueEmail(string email)
		{
			return await this.repos.UniqueEmail(email);
		}
	}
}
