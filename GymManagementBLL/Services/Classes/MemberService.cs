using AutoMapper;
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
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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

                // CreateMemberViewModel => Member ===> Mapping
                var member = _mapper.Map<CreateMemberViewModel, Member>(createMember);

                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
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
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;

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

                var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(ms => ms.MemberId == MemberId && ms.Session.StartDate > DateTime.Now).Any();

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
                return _unitOfWork.SaveChanges() > 0;

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
