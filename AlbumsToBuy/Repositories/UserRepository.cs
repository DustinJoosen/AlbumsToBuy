using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class UserRepository : CrudRepository<User>
	{
		public UserRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
