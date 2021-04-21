using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Payment : IBase
	{
		public int Id { get; set; }

		public User User { get; set; }
		[Required]
		public int UserId { get; set; }

		[DataType("decimal(16,2)")]
		[Required]
		public decimal Amount { get; set; }

		[Display(Name = "Payment Status")]
		public PaymentStatus Status { get; set; }
	}
}
