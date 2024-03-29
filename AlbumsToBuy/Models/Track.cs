﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

		[Display(Name="Duration in seconds")]
		public int Duration { get; set; }

		[NotMapped]
		public string FormattedDuration
		{
			get
			{
				return $"{Convert.ToString(100 + Duration / 60).Substring(1)}:{Convert.ToString(100 + Duration % 60).Substring(1)}";
			}
		}
	}
}
