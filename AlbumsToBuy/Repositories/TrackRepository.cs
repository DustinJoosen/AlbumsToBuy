﻿using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class TrackRepository : CrudRepository<Track>
	{
		public TrackRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
