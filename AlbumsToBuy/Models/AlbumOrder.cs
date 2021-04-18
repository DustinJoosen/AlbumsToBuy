using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class AlbumOrder : IBase
	{
		public int Id { get; set; }

		public Album Album { get; set; }
		[Required]
		public int AlbumId { get; set; }

		public Order Order { get; set; }
		[Required]
		public int OrderId { get; set; }


		public int Quantity { get; set; }
	}
}
