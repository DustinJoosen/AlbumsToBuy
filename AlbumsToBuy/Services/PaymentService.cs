using AlbumsToBuy.Dtos;
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

		public async Task<int> Count(bool includePaid)
		{
			return await this._repos.Count(includePaid);
		}
		public async Task<List<Payment>> GetUnpaid()
		{
			return await this._repos.GetUnpaid();
		}
		public async Task<List<Payment>> GetByPage(PaginationDto pagination, bool showDelivered)
		{
			return await this._repos.GetByPage(pagination, showDelivered);
		}
	}
}
