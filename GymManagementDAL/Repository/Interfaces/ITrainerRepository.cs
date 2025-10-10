using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repository.Interfaces
{
    internal interface ITrainerRepository
    {
        IEnumerable<Trainer> GetAll();
        Trainer? GetById(int id);
        int Add(Trainer trainer);
        int Update(Trainer trainer);
        int Delete(int id);

    }
}
