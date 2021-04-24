using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class ShoppingListItemRepository : CrudRepository<ShoppingListItem>
	{
		private ApplicationDbContext _context;
		public ShoppingListItemRepository(ApplicationDbContext context) : base(context)  
		{
			_context = context;
		}

		public override async Task<ShoppingListItem> GetById(int id)
		{
			return await _context.ShoppingListItems
				.Include(s => s.Album)
				.SingleOrDefaultAsync(s => s.Id == id);
		}

		public override async Task Create(ShoppingListItem model)
		{
			var shoppinglistitem = await _context.ShoppingListItems.SingleOrDefaultAsync(s => s.AlbumId == model.AlbumId && s.UserId == model.UserId);
			if (shoppinglistitem == null)
			{
				await base.Create(model);
				return;
			}

			shoppinglistitem.Quantity++;
			await base.Update(shoppinglistitem);
		}

		public async Task RemoveFromUser(int id)
		{
			var shoppingListItems = await _context.ShoppingListItems.Where(s => s.UserId == id).ToListAsync();
			foreach(var shoppingListItem in shoppingListItems)
			{
				_context.ShoppingListItems.Remove(shoppingListItem);
			}

			await _context.SaveChangesAsync();
		}
	}
}
