using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Interfaces
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        IEnumerable<MemberShip> GetMemberShipsWithMemberAndPlan(Func<MemberShip,bool>? filter = null);

        MemberShip? GetFirstMemberShip(Func<MemberShip, bool>? filter = null);
    }
}
