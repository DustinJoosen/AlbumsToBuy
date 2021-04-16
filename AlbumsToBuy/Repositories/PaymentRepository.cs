using AlbumsToBuy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public class PaymentRepository : CrudRepository<Payment>
	{
		public PaymentRepository(ApplicationDbContext context) : base(context)
		{

		}
	}
}
