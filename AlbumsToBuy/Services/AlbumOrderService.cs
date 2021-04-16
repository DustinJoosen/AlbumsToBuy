using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class AlbumOrderService : CrudService<AlbumOrder>
	{
		public AlbumOrderService(AlbumOrderRepository repos) : base(repos)
		{

		}
	}
}
