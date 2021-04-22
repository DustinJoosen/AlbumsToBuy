using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Order : IBase
	{
		public int Id { get; set; }

		public Payment Payment { get; set; }
		[Required]
		public int PaymentId { get; set; }

		[Required]
		public string Street { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Country { get; set; }

		[Required]
		[Display(Name ="Zip Code")]
		public string ZipCode { get; set; }

		public User User { get; set; }
		[Required]
		[Display(Name = "User")]
		public int UserId { get; set; }

		[Display(Name = "Order Date")]
		public DateTime OrderDate { get; set; } = DateTime.Now;

		[Display(Name = "Order Status")]
		public OrderStatus Status { get; set; } = OrderStatus.Recieved;

		public string SpecialNotes{ get; set; }

		public List<AlbumOrder> Albums { get; set; }
	}
}
