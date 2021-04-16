﻿using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class AddressRepository : CrudRepository<Address>
	{
		public AddressRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
