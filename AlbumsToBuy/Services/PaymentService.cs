using AlbumsToBuy.Models;
using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public class PaymentService : CrudService<Payment>
	{
		private PaymentRepository _repos;
		public PaymentService(PaymentRepository repos) : base(repos)
		{
			_repos = repos;
		}

		public async Task<List<Payment>> GetUnpaid()
		{
			return await this._repos.GetUnpaid();
		}
	}
}
