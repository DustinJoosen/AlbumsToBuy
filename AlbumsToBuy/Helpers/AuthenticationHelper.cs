using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Helpers
{
	public static class AuthenticationHelper
	{


		public static string CreateToken()
		{
			return new Password()
				.IncludeLowercase()
				.IncludeUppercase()
				.IncludeNumeric()
				.IncludeSpecial()
				.LengthRequired(32)
				.Next();
		}
	}
}
