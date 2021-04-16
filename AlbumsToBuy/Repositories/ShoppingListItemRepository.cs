using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class ShoppingListItemRepository : CrudRepository<ShoppingListItem>
	{
		public ShoppingListItemRepository(ApplicationDbContext context) : base(context)  
		{

		}
	}
}
