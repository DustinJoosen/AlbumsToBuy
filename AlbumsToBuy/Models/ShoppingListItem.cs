using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class ShoppingListItem : IBase
	{
		public int Id { get; set; }

		[Required]
		public User User { get; set; }
		public int UserId { get; set; }

		[Required]
		public Album Album { get; set; }
		public int AlbumId { get; set; }
	}
}
