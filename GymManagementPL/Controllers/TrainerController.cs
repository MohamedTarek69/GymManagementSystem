using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #endregion

        #region Get Trainers Details
        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID Of Trainer Cannot Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        #endregion

        #region Create Trainer

        public ActionResult CreateTrainer()
        {
            return View();
        }

        public ActionResult CreateTrainerSubmit(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please fill in all required fields");
                return View(nameof(CreateTrainer), createTrainer);
            }

            bool Result = _trainerService.CreateTrainer(createTrainer);
            if (Result)
            {
                TempData["SuccessMassage"] = "Trainer Created Successfully";

            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Create Trainer , Check Phone And Name";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Update Trainer
        public ActionResult UpdateTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID Of Trainer Cannot Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult UpdateTrainer([FromRoute] int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID Of Trainer Cannot Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please fill in all required fields");
                return View(nameof(UpdateTrainer), trainerToUpdate);
            }
            bool Result = _trainerService.UpdateTrainerDetails(id, trainerToUpdate);
            if (Result)
            {
                TempData["SuccessMassage"] = "Trainer Updated Successfully";
            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Update Trainer , Check Phone And Name";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "ID Of Trainer Cannot Be 0 Or Nigative Number";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMassage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Trainerid = id;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            bool Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMassage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Delete Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
