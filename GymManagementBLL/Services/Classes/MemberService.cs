using AutoMapper;
using GymManagementBLL.Services.AttachmentService.Interface;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Classes;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper , IAttachmentService attachmentService) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService=attachmentService;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();

            if (Members == null || !Members.Any()) return [];

            // Member => MemberViewModel ===> Mapping
            #region Way 01
            //var MemberViewModels = new List<MemberViewModel>();
            //foreach (var member in Members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Gender = member.Gender.ToString(),
            //        Photo = member.Photo
            //    };
            //    MemberViewModels.Add(memberViewModel);
            //}

            //return MemberViewModels;

            #endregion

            #region Way 02
            var MemberViewModels = Members.Select(member => _mapper.Map<MemberViewModel>(member));

            return MemberViewModels;

            #endregion

            //return members;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {

                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;

                var PhotoName = _attachmentService.UploadImage("Members", createMember.Photo);
                if (string.IsNullOrEmpty(PhotoName)) return false;

                // CreateMemberViewModel => Member ===> Mapping
                var member = _mapper.Map<CreateMemberViewModel, Member>(createMember);
                member.Photo = PhotoName;

                _unitOfWork.GetRepository<Member>().Add(member);
                var IsCreated = _unitOfWork.SaveChanges() > 0;
                if (!IsCreated)
                {
                    _attachmentService.DeleteImage(PhotoName, "Members"); 
                }
                return IsCreated;
            }
            catch
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);

            if (member == null) return null;

           // Member => MemberViewModel ===> Mapping
            var memberViewModel = _mapper.Map<Member, MemberViewModel>(member);

            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>()
                             .GetAll(ms => ms.MemberId == MemberId && ms.Status == "Active")
                             .FirstOrDefault();
            if (ActiveMemberShip != null)
            {
                memberViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                if (plan != null)
                {
                    memberViewModel.PlanName = plan.Name;
                }
            }

                return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord == null) return null;

            // HealthRecord => HealthRecordViewModel ===> Mapping
            var healthRecordViewModel = _mapper.Map<HealthRecord, HealthRecordViewModel>(MemberHealthRecord);

            return healthRecordViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return _mapper.Map<Member, MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {

                var emailExsists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == memberToUpdate.Email && X.Id != MemberId).Any();

                var phoneExsists = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == memberToUpdate.Phone && X.Id != MemberId).Any();

                if (emailExsists || phoneExsists) return false;

                var Repo = _unitOfWork.GetRepository<Member>();

                var member = Repo.GetById(MemberId);
                if (member == null) return false;

                _mapper.Map(memberToUpdate, member);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveMember(int MemberId)
        {
            try
            {
                var MemberRepo = _unitOfWork.GetRepository<Member>();

                var member = MemberRepo.GetById(MemberId);
                if (member == null) return false;

                var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(ms => ms.MemberId == MemberId).Select(ms=>ms.SessionId);
                // 1 , 2 , 3

                var HasActiveMemberSession = _unitOfWork.GetRepository<Session>()
                    .GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();



                if (HasActiveMemberSession) return false;

                var MemeberShipRepo = _unitOfWork.GetRepository<MemberShip>();

                var memberShip = MemeberShipRepo.GetAll(ms => ms.MemberId == MemberId);
                if (memberShip.Any())
                {
                    foreach (var ms in memberShip)
                    {
                        MemeberShipRepo.Delete(ms);
                    }
                }

                MemberRepo.Delete(member);
                var IsDeleted = _unitOfWork.SaveChanges() > 0;
                if (IsDeleted)
                {
                    _attachmentService.DeleteImage(member.Photo, "Members");
                }
                return IsDeleted;

            }
            catch
            {
                return false;
            }
        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string phone) {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

        #endregion
    }
}
