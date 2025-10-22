using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            this._memberService = memberService;
        }

        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region Get Member Details
        //BaseUrl/Member/MemberDetails/1

        public ActionResult MemberDetails(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMassage"] = "ID Of Member Cannot Be 0 Or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["ErrorMassage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }   

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "ID Of Member Cannot Be 0 Or Nigative Number";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);

            if (healthRecord is null)
            {
                TempData["ErrorMassage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }

        #endregion

        #region Add Member
       
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please fill in all required fields");
                return View(nameof(Create), createMember);
            }

            bool Result = _memberService.CreateMember(createMember);
            if (Result)
            {
                TempData["SuccessMassage"] = "Member Created Successfully";
                
            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Create Member , Check Phone And Name";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update Member

        public ActionResult Edit(int id) 
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "Id Of Member Cannot Be 0 Or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberToUpdate(id);

            if (member is null)
            {
                TempData["ErrorMassage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute]int id ,MemberToUpdateViewModel member) 
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Please fill in all required fields");
                return View(nameof(Edit), member);
            }
            bool Result = _memberService.UpdateMemberDetails(id,member);
            if (Result)
            {
                TempData["SuccessMassage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Update Member , Check Phone And Name";
            }

            return RedirectToAction(nameof(Index));
        }



        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMassage"] = "ID Of Member Cannot Be 0 Or Nigative Number";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMassage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }

           ViewBag.Memberid = id;

            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            bool Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMassage"] = "Member Deleted Successfully";
            }
            else
            {
                TempData["ErrorMassage"] = "Failed To Delete Member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
