using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithTrainersAndCategories();

        int GetCountOfBookedSlots(int sessionId);

        Session? GetSessionByIdWithTrainersAndCategories(int sessionId);

    }
}
