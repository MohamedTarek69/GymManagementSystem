using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return [];

            return plans.Select(plan => new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            });
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null) return null;

            return new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price,
                IsActive = plan.IsActive
            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || !plan.IsActive || HasActiveMemberShips(planId)) return null;


            return new UpdatePlanViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price = plan.Price
            };
        }

        public bool UpdatePlanDetails(int planId, UpdatePlanViewModel updatedPlan)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();

            var plan = PlanRepo.GetById(planId);
            if (plan is null || HasActiveMemberShips(planId)) return false;

            (plan.Name, 
             plan.Description,
             plan.DurationDays,
             plan.Price,
             plan.UpdatedAt) = 
                          (updatedPlan.PlanName,
                           updatedPlan.Description, 
                           updatedPlan.DurationDays,
                           updatedPlan.Price,
                           DateTime.Now);

            PlanRepo.Update(plan);

            return _unitOfWork.SaveChanges() > 0;
        }

        public bool ToggleStatus(int planId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(planId);
            if (plan is null || HasActiveMemberShips(planId)) return false;

            //plan.IsActive = !plan.IsActive;
            plan.IsActive = plan.IsActive == true ? false : true;
            try
            {
                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }


        }


        #region Helper Methods
        private bool HasActiveMemberShips(int planId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                              .GetAll(ms => ms.PlanId == planId && ms.Status == "Active")
                              .Any();
        }

        #endregion
    }
}
