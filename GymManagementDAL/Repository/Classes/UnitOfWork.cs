using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext, ISessionRepository sessionRepository) 
        {
            _dbContext = dbContext;
            this.sessionRepository = sessionRepository;
        }
        private readonly Dictionary<Type, object> _repositories = new();

        public ISessionRepository sessionRepository { get; }

        // Key : Type of Entity (Member , Trainer , Session)
        // Value : Repository Instance (GenericRepository<Member> , GenericRepository<Trainer> , GenericRepository<Session>)
        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var Entitytype = typeof(TEntity);
            if (_repositories.ContainsKey(Entitytype))
            {
                return (IGenericRepository<TEntity>) _repositories[Entitytype];
            }

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[Entitytype] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
