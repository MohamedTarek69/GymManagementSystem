using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainersAndCategories()
        {
            return _dbContext.Sessions
                             .Include(s => s.SessionTrainer)
                             .Include(s => s.SessionCategory)
                             .AsNoTracking()
                             .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(MS => MS.SessionId == sessionId);
        }

        public Session? GetSessionByIdWithTrainersAndCategories(int sessionId)
        {
            return _dbContext.Sessions
                             .Include(s => s.SessionTrainer)
                             .Include(s => s.SessionCategory)
                             .AsNoTracking()
                             .FirstOrDefault(s => s.Id == sessionId);
        }
    }
}
