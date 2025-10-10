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
    internal class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext;

        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(Plan plan)
        {
            _dbContext.Plans.Add(plan);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var plan = _dbContext.Plans.Find(id);
            
            if (plan == null) return 0;

            _dbContext.Plans.Remove(plan);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<Plan> GetAll() => _dbContext.Plans.ToList();

        public Plan? GetById(int id) => _dbContext.Plans.Find(id);

        public int Update(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
