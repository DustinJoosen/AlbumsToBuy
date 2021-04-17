using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.ViewModels
{
	public class OrderAlbums
	{
		public Order Order { get; set; }
		public List<AlbumOrder> AlbumOrders { get; set; }
	}
}
