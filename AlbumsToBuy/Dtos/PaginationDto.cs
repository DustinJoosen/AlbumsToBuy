using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class PaginationDto
	{
		public int PageSize { get; set; }
		public int PageNumber { get; set; }
		public int TotalPages { get; set; }
	}
}
