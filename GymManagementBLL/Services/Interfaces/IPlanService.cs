using GymManagementBLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanDetails(int planId);
        UpdatePlanViewModel? GetPlanToUpdate(int planId);
        bool UpdatePlanDetails(int planId, UpdatePlanViewModel updatedPlan);
        bool ToggleStatus(int planId);
    }
}
