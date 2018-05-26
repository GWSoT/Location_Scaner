using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eleks_2018_MicroSocialMedia.Repositories.Interfaces
{
    public interface IRepository<TEntity, TPrimaryKey>
    {
        TEntity Find(TPrimaryKey x);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        IEnumerable<TEntity> All();
        void Add(TEntity entity);
        void SaveChanges();
    }
}
