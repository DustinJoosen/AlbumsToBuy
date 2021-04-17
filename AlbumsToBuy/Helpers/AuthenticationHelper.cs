using AlbumsToBuy.Dtos;
using AlbumsToBuy.Models;
using AlbumsToBuy.Services;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using PasswordGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlbumsToBuy.Helpers
{
	public class AuthenticationHelper
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
