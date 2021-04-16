using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Track : IBase
	{
		public int Id { get; set; }

		[Required]
		public int AlbumId { get; set; }

		[Required]
		public string Name { get; set; }
		public int Duration { get; set; }
	}
}
