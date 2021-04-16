using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Models
{
	public class Order
	{
		public int Id { get; set; }

		[Required]
		public Payment Payment { get; set; }
		public int PaymentId { get; set; }
		
		[Required]
		public Address Address { get; set; }
		public int AddressId { get; set; }
		
		[Required]
		public User User { get; set; }
		public int UserId { get; set; }

		public DateTime OrderDate { get; set; } = DateTime.UtcNow;
		public OrderStatus Status { get; set; } = OrderStatus.Recieved;
		public string SpecialNotes{ get; set; }
	}
}
