using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class User : IBase
	{
		public int Id { get; set; }
		
		[Required]
		public string FirstName { get; set; }
		
		[Required]
		public string LastName { get; set; }
		
		[Required]
		public int HomeAddressId { get; set; }
		public Address HomeAddress { get; set; }
		
		[Required]
		public string Email { get; set; }
		
		[Required]
		public string Password { get; set; }
		
		[Required]
		public UserRole Role { get; set; }

		public string UserToken { get; set; }

		public List<ShoppingListItem> ShoppingListItems { get; set; }
	}
}
