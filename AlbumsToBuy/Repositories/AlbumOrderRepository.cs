using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class AlbumOrderRepository : CrudRepository<AlbumOrder>
	{
		public AlbumOrderRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
