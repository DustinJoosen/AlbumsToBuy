using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public abstract class CrudRepository<T> where T : class
	{
		private ApplicationDbContext _context;
		private DbSet<T> _entity;

		public CrudRepository(ApplicationDbContext context)
		{
			_context = context;
			_entity = context.Set<T>();
		}

		public async Task<List<T>> GetAll()
		{
			return await this._entity.ToListAsync();
		}

		public async Task<T> GetById(int id)
		{
			return await this._entity.FindAsync(id);
		}

		public async Task Create(T model)
		{
			this._entity.Add(model);
			await this._context.SaveChangesAsync();
		}

		public async Task Update(T model)
		{
			this._context.Entry(model).State = EntityState.Modified;
			await this._context.SaveChangesAsync();
		}

		public async Task Remove(T model)
		{
			this._entity.Remove(model);
			await this._context.SaveChangesAsync();
		}
	}
}
