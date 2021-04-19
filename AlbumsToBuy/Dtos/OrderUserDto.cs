using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class OrderUserDto
	{
		public Order Order { get; set; }
		public int OrderId { get; set; }

		public User User { get; set; }
		public int UserId { get; set; }

		public decimal TotalPrice{ get; set; }
	}
}
