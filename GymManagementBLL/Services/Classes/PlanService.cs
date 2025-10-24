using AutoMapper;
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
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans == null || !plans.Any()) return [];

            var MappedPlans = _mapper.Map<IEnumerable<Plan>, IEnumerable<PlanViewModel>>(plans);

            return MappedPlans;
        }

        public PlanViewModel? GetPlanDetails(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan == null) return null;

            var MappedPlan = _mapper.Map<Plan, PlanViewModel>(plan);
            return MappedPlan;
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int planId)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(planId);
            if (plan is null || !plan.IsActive || HasActiveMemberShips(planId)) return null;

            var MappedPlan = _mapper.Map<Plan, UpdatePlanViewModel>(plan);
            return MappedPlan;

        }

        public bool UpdatePlanDetails(int planId, UpdatePlanViewModel updatedPlan)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();

            var plan = PlanRepo.GetById(planId);
            if (plan is null || HasActiveMemberShips(planId)) return false;

            _mapper.Map(updatedPlan, plan);

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
