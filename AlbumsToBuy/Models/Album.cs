using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Album
	{
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }
		
		[Required]
		public string Creator { get; set; }
		
		[Required]
		[DataType("decimal(16,2)")]
		public decimal Price { get; set; }
		
		[Required]
		public int Stock { get; set; }
		
		[Required]
		public DateTime ReleaseDate { get; set; }
		public string CoverImage { get; set; }
		
		[Required]
		public AlbumType Type { get; set; }

		public List<Track> Tracks { get; set; }
	}
}
