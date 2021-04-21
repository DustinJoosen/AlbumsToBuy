using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class User : IBase
	{
		public int Id { get; set; }
		
		[Required]
		[Display(Name="First Name")]
		public string FirstName { get; set; }
		
		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
		
		[Required]
		public string Email { get; set; }
		
		[Required]
		public string Password { get; set; }

		public string Street { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		[Display(Name="Zip Code")]
		public string ZipCode { get; set; }

		[Required]
		public UserRole Role { get; set; }

		public List<ShoppingListItem> ShoppingListItems { get; set; }

		[NotMapped]
		[Display(Name = "Full Name")]
		public string FullName { get { return $"{FirstName} {LastName}";  } }
	}
}
