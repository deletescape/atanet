namespace Atanet.Services.Repository
{
    using Atanet.DataAccess.Context;
    using Atanet.Model.Interfaces;
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public class Repository<T> : IRepository<T> where T : class, IIdentifiable
    {
        private readonly AtanetDbContext atanetDbContext;

        public Repository(AtanetDbContext atanetDbContext) =>
            this.atanetDbContext = atanetDbContext;

        public T Create(T entity)
        {
            var entry = this.atanetDbContext.Add(entity);
            return entry.Entity;
        }

        public void Delete(Expression<Func<T, bool>> func)
        {
            var results = this.Query().Where(func).ToArray();
            foreach (var result in results)
            {
                this.atanetDbContext.Remove(result);
            }
        }

        public void Delete(long id)
        {
            var entity = this.FindById(id);
            if (entity != null)
            {
                this.atanetDbContext.Set<T>().Remove(entity);
            }
        }

        public T FindById(long id) =>
            this.Query().FirstOrDefault(x => x.Id == id);

        public IQueryable<T> Query() =>
            this.atanetDbContext.Set<T>();

        public T Update(T entity)
        {
            this.atanetDbContext.Set<T>().Update(entity);
            return entity;
        }
    }
}
