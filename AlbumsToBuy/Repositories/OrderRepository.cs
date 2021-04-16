using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class OrderRepository : CrudRepository<Order>
	{
		public OrderRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
