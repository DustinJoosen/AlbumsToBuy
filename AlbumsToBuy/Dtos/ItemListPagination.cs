using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class ItemListPagination
	{
		public List<Order> Orders { get; set; }
		public List<Payment> Payments { get; set; }
		public PaginationDto Pagination { get; set; }
	}
}
