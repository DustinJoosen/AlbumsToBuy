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
		private OrderRepository _repos;
		public OrderService(OrderRepository repos) : base(repos)
		{
			_repos = repos;
		}

		public async Task<List<Order>> GetByUserId(int id)
		{
			return await this._repos.GetByUserId(id);
		}

	}
}
