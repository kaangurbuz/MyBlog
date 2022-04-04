using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyNotes.CommonLayer;
using MyNotes.CoreLayer;
using MyNotes.EntityLayer;

namespace MyNotes.DataAccessLayer
{
    public class Repository<T>:BaseRepository,IRepository<T> where T :  class
    {
        private readonly MyNoteContext _db;
        private DbSet<T> objSet; 

        public Repository()
        {
            _db = BaseRepository.CreateContext();
            objSet = _db.Set<T>();
        }
        public List<T> List() // Bircok programda list yerine GetAll kullanılır.
        {
            return objSet.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> predicate)
        {
            return objSet.Where(predicate).ToList();
        }

        public IQueryable<T> QList()
        {
            return objSet.AsQueryable();
        }

        public int Insert(T entity)
        {
            objSet.Add(entity);
            if (entity is BaseEntity obj)
            {
                DateTime now = DateTime.Now;
                //BaseEntity o = entity as BaseEntity;
                obj.ModifiedUserName = "system";
                obj.CreatedOn = now;
                obj.ModifiedOn = now;
            }
            return Save();
        }

        public int Update(T entity)
        {
            if (entity is BaseEntity o)
            {
                o.ModifiedUserName = App.Common.GetCurrentUsername();
                o.ModifiedOn= DateTime.Now;
            }
            return Save();
        }

        public int Delete(T entity)
        {
            objSet.Remove(entity);
            return Save();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> predicate)
        {
            return objSet.FirstOrDefault(predicate);
        }
    }
}
