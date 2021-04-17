using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class AlbumService : CrudService<Album>
	{
		private AlbumRepository _repos;

		public AlbumService(AlbumRepository repos) : base(repos)
		{
			_repos = repos;
		}

		public async Task<List<AlbumOrder>> GetByOrder(Order order)
		{
			return await this._repos.GetByOrder(order);
		}
	}
}
