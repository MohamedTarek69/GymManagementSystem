using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index");

            }
            var session = _sessionService.GetSessionById(id);
            if(session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            return View(session);

        }

        public ActionResult Create()
        {

            LoadTrainersDropDowns();
            LoadCategoriesDropDowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(createSession);
            }
            var result = _sessionService.CreateSession(createSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create session.";
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(createSession);
            }
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction("Index");
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session == null) {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }
            LoadTrainersDropDowns();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                return View(updateSession);
            }
            var result = _sessionService.UpdateSession(id,updateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session.";
                LoadTrainersDropDowns();
                return View(updateSession);
            }
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "ID Of Session Cannot Be 0 Or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = id;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Session";
            }
            return RedirectToAction(nameof(Index));
        }


        #region Helper Methods

        public void LoadTrainersDropDowns()
        {
            var trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        public void LoadCategoriesDropDowns()
        {
            var categories = _sessionService.GetAllCategoriesForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        #endregion

    }
}
