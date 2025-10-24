using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction("Index");
            }
            var plan = _planService.GetPlanDetails(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        public ActionResult Edit(int id)
        {
           if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction("Index");
            }
            var plan = _planService.GetPlanToUpdate(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Failed to edit plan.";
                return RedirectToAction("Index");
            }
            return View(plan);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id, UpdatePlanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Please Check The Validation Of The Data");
                return View(model);
            }

            var result = _planService.UpdatePlanDetails(id, model);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to update plan.";
                return View(model);
            }
            else
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Activate(int id)
        {
            if (id <= 0) {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction("Index");
            }
            var result = _planService.ToggleStatus(id);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to update plan status.";
            }
            else
            {
                TempData["SuccessMessage"] = "Plan status updated successfully.";
            }
            return RedirectToAction("Index");
        }

        #region If both action do the same the same 
        //public ActionResult Details(int id, string ViewName = "Details")
        //   {
        //       if (id <= 0)
        //       {
        //           TempData["ErrorMessage"] = "Invalid plan ID.";
        //           return RedirectToAction("Index");
        //       }
        //       var plan = _planService.GetPlanDetails(id);
        //       if (plan == null)
        //       {
        //           TempData["ErrorMessage"] = "Plan not found.";
        //           return RedirectToAction("Index");
        //       }
        //       return View(ViewName, plan);
        //   }

        //public ActionResult Edit(int id)
        //   {
        //       //if(id <= 0)
        //       // {
        //       //     TempData["ErrorMessage"] = "Invalid plan ID.";
        //       //     return RedirectToAction("Index");
        //       // }
        //       // var plan = _planService.GetPlanToUpdate(id);
        //       // if (plan == null)
        //       // {
        //       //     TempData["ErrorMessage"] = "Plan not found.";
        //       //     return RedirectToAction("Index");
        //       // }
        //       // return View(plan);
        //       return Details(id, "Edit");
        //   }

        #endregion
    }


}
