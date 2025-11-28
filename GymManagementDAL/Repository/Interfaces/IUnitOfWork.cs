using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public ISessionRepository sessionRepository { get; }
        IMemberShipRepository MembershipRepository { get; }
        IBookingRepository BookingRepository { get; }

        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new(); 

        int SaveChanges();
    }
}
