using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class ShopSearchDto
	{
		public string SearchValue { get; set; }
		public ShopSearchType SearchType { get; set; }
	}
}
