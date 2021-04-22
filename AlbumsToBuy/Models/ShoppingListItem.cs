using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class ShoppingListItem : IBase, IAlbumCollection
	{
		public int Id { get; set; }

		public User User { get; set; }
		[Required]
		public int UserId { get; set; }

		public Album Album { get; set; }
		[Required]
		public int AlbumId { get; set; }

		public int Quantity { get; set; }
	}
}
