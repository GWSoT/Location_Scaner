using Eleks_2018_MicroSocialMedia.Data;
using Eleks_2018_MicroSocialMedia.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories
{
    public class BaseRepository<TEntity, TPrimaryKey> 
        : IDisposable, IRepository<TEntity, TPrimaryKey> 
            where TEntity : class

    {
        protected DbSet<TEntity> DbSet;
        protected MSMContext _dbContext;

        public BaseRepository(MSMContext context)
        {
            DbSet = context.Set<TEntity>();
            _dbContext = context;
        }

        public void Add(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public IEnumerable<TEntity> All()
        {
            return DbSet;
        }

        public virtual TEntity Find(TPrimaryKey x)
        {
            return DbSet.Find(x);
        }

        public void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
