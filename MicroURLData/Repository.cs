using System.Linq.Expressions;

namespace MicroURLData {
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
        protected readonly List<TEntity> Context;
        protected Repository(DbContext context)
        {
            Context = (List<TEntity>)context.GetDefault(GetType(), new List<TEntity>());
        }
        public void Add(TEntity entity) {
            Context.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities) {
            Context.AddRange(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate) {
            return Context.Where(predicate.Compile()).ToList();
        }

        public abstract TEntity Get(string id);

        public IEnumerable<TEntity> GetAll() {
            return Context;
        }

        public void Remove(TEntity entity) {
            Context.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities) {
            foreach (var entity in entities) {
                Context.Remove(entity);
            }
        }
    }
}
