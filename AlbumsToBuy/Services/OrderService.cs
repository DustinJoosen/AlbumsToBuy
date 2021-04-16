using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class OrderService : CrudService<Order>
	{
		public OrderService(OrderRepository repos) : base(repos)
		{

		}
	}
}
