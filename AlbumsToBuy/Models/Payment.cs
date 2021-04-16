using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Payment
	{
		public int Id { get; set; }

		[Required]
		public User User { get; set; }
		public int UserId { get; set; }

		[Required]
		public int Amount { get; set; }
		public PaymentStatus Status { get; set; }
	}
}
