using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class AlbumOrder
	{
		public int Id { get; set; }

		[Required]
		public Album Album { get; set; }
		public int AlbumId { get; set; }

		[Required]
		public Order Order { get; set; }
		public int OrderId { get; set; }
	}
}
