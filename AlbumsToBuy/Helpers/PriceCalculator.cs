using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Helpers
{
	public class PriceCalculator
	{
		public static decimal CalculateShoppingList(List<ShoppingListItem> items)
		{
			decimal price = 0;
			foreach (var item in items)
			{
				price += (item.Album.Price * item.Quantity);
			}
			return price;
		}
	}
}
