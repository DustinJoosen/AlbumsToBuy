using AlbumsToBuy.Dtos;
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

		public async Task<int> Count(bool includeDelivered)
		{
			return await this._repos.Count(includeDelivered);
		}

		public async Task<List<Order>> GetByUserId(int id)
		{
			return await this._repos.GetByUserId(id);
		}

		public async Task<List<Order>> GetByPage(PaginationDto pagination, bool showDelivered)
		{
			return await this._repos.GetByPage(pagination, showDelivered);
		}
	}
}
