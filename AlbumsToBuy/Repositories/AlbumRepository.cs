using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class AlbumRepository : CrudRepository<Album>
	{
		public AlbumRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
