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
		public UserService(UserRepository repos) : base(repos)
		{

		}
	}
}
