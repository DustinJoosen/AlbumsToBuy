using AlbumsToBuy.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbumsToBuy.Services
{
	public abstract class CrudService<T> where T : class
	{
        protected readonly CrudRepository<T> _repository;
        public CrudService(CrudRepository<T> genericRepository)
        {
            _repository = genericRepository;
        }
        public virtual async Task<T> GetById(int id)
        {
            return await _repository.GetById(id);
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _repository.GetAll();
        }

        public virtual async Task Create(T entity)
        {
            await _repository.Create(entity);
        }

        public virtual async Task Update(T entity)
        {
            await _repository.Update(entity);
        }

        public virtual async Task Remove(T entity)
        {
            await _repository.Remove(entity);
        }
    }
}
