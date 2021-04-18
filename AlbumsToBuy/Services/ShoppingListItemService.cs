using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class ShoppingListItemService : CrudService<ShoppingListItem>
	{
		private ShoppingListItemRepository _repos;
		public ShoppingListItemService(ShoppingListItemRepository repos) : base(repos)
		{
			_repos = repos;
		}

		public async Task RemoveFromUser(int id)
		{
			await _repos.RemoveFromUser(id);
		}
	}
}
