using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class AddressService : CrudService<Address>
	{
		public AddressService(AddressRepository repos) : base(repos)
		{

		}
	}
}
