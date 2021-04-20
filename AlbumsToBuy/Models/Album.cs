using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Album : IBase
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
		[Display(Name = "Release Date")]
		public DateTime ReleaseDate { get; set; }

		[Display(Name = "Cover Image")]
		public string CoverImage { get; set; }
		
		[Required]
		public AlbumType Type { get; set; }

		public List<Track> Tracks { get; set; }

		[NotMapped]
		[Display(Name = "Cover Image")]
		public IFormFile FormFile { get; set; }

		[NotMapped]
		public string ImageUrl
		{
			get
			{
				if(CoverImage == "NotFound.png")
					return "upload/NotFound.png";
				
				return $"upload/album/{CoverImage}";
			}
		}
	}
}
