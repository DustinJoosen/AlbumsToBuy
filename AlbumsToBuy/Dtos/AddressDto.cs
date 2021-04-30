using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class AddressDto
	{
		[Required]
		public string Street { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		[Display(Name = "Zip Code")]
		public string ZipCode { get; set; }
	}
}
