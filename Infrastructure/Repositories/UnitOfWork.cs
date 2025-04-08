using Application.IRepositories;
using Domain.Models;
using Infrastructure.Context;
using Infrastructure.IRepositories;
using Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FoodAppDbContext _dbContext;
        private readonly Hashtable _repository;

        public UnitOfWork(FoodAppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repository = new Hashtable();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repository.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbContext);
                _repository.Add(type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)_repository[type];

        }
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
