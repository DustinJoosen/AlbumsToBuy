using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Dtos
{
	public class RegisterDto
	{
		[Required]
		public string Email { get; set; }
		
		[Required]
		public string Password { get; set; }
		
		[Required]
		[Display(Name = "Password again. For confirmation")]
		public string PasswordConfirmation { get; set; }
		
		[Required]
		[Display(Name = "First Name")]
		public string FirstName { get; set; }
		
		
		[Required]
		[Display(Name = "Last Name")]
		public string LastName { get; set; }
	}
}
