using AlbumsToBuy.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Repositories
{
	public abstract class CrudRepository<T> where T : class, IBase
	{
		private ApplicationDbContext _context;
		private DbSet<T> _entity;

		public CrudRepository(ApplicationDbContext context)
		{
			_context = context;
			_entity = context.Set<T>();
		}

		public virtual async Task<List<T>> GetAll()
		{
			return await this._entity.ToListAsync();
		}

		public virtual async Task<T> GetById(int id)
		{
			return await this._entity.FindAsync(id);
		}

		public virtual async Task Create(T model)
		{
			model.Id = 0;

			this._entity.Add(model);
			await this._context.SaveChangesAsync();
		}

		public virtual async Task Update(T model)
		{
			this._context.Entry(model).State = EntityState.Modified;
			await this._context.SaveChangesAsync();
		}

		public virtual async Task Remove(T model)
		{
			this._entity.Remove(model);
			await this._context.SaveChangesAsync();
		}

		public virtual bool Exists(T model)
		{
			return this._entity.Any(s => s.Id == model.Id);
		}
	}
}
