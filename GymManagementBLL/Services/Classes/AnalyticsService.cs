using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
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
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            var SessionRepo = _unitOfWork.sessionRepository;
            return new AnalyticsViewModel
            {

                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll().Count(m => m.Status =="Active"),
                UpcomingSessions = SessionRepo.GetAll().Count(s => s.StartDate > DateTime.Now),
                OngoingSessions = SessionRepo.GetAll().Count(s => s.StartDate <= DateTime.Now && s.EndDate > DateTime.Now),
                CompletedSessions = SessionRepo.GetAll().Count(s => s.EndDate < DateTime.Now)
            };
        }
    } 
}
