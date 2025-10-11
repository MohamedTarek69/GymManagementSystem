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

        public MemberService(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
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
            var MemberViewModels = Members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString()
            });

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
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Notes = createMember.HealthRecordViewModel.Notes
                    }
                };

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
            var memberViewModel = new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}"
            };

            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>()
                             .GetAll(ms => ms.MemberId == MemberId && ms.Status == "Active").FirstOrDefault();
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
            var healthRecordViewModel = new HealthRecordViewModel()
            {
                Height = MemberHealthRecord.Height,
                Weight = MemberHealthRecord.Weight,
                BloodType = MemberHealthRecord.BloodType,
                Notes = MemberHealthRecord.Notes
            };

            return healthRecordViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            return new MemberToUpdateViewModel()
            {
                Name = member.Name,
                Photo = member.Photo,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
        }

        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;

                var Repo = _unitOfWork.GetRepository<Member>();

                var member = Repo.GetById(MemberId);
                if (member == null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.Street = memberToUpdate.Street;
                member.Address.City = memberToUpdate.City;
                member.UpdatedAt = DateTime.Now;

                Repo.Update(member);
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
