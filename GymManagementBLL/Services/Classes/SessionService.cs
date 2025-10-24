using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.sessionRepository.GetAllSessionsWithTrainersAndCategories();
            if (Sessions == null || !Sessions.Any()) return [];

            #region Manual Mapping
            //// Session -> SessionViewModel ===> Manual Mapping
            //// Library => AutoMapper

            //return Sessions.Select(session => new SessionViewModel()
            //{
            //    Id = session.Id,
            //    Capacity = session.Capacity,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    TrainerName = session.SessionTrainer.Name,
            //    CategoryName = session.SessionCategory.CategoryName,
            //    AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id)
            //}); 

            #endregion

            #region Automatic Mapping
            var MappedSessions = _mapper.Map<IEnumerable<Session>,IEnumerable<SessionViewModel>>(Sessions);

            return MappedSessions;

            #endregion

        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            var session = _unitOfWork.sessionRepository.GetSessionByIdWithTrainersAndCategories(sessionId);
            if (session == null) return null;

            #region Manual Mapping
            //return new SessionViewModel()
            //{
            //    Capacity = session.Capacity,
            //    Description = session.Description,
            //    StartDate = session.StartDate,
            //    EndDate = session.EndDate,
            //    TrainerName = session.SessionTrainer.Name,
            //    CategoryName = session.SessionCategory.CategoryName,
            //    AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id)
            //}; 

            #endregion

            #region Automatic Mapping
            var MappedSession = _mapper.Map<Session, SessionViewModel>(session);
            MappedSession.AvailableSlots = session.Capacity - _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id);

            return MappedSession;

            #endregion
        }
        public bool CreateSession(CreateSessionViewModel createSession)
        {
            if(!IsTrainerExsists(createSession.TrainerId)) return false;
            if(!IsCategoryExsists(createSession.CategoryId)) return false;
            if(!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;

            // CreateSessionViewModel -> Session
            var MappedSessions = _mapper.Map<CreateSessionViewModel, Session>(createSession);

            _unitOfWork.GetRepository<Session>().Add(MappedSessions);
            return _unitOfWork.SaveChanges() > 0;


        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (!IsSessionAvilableForUpdating(session!)) return null;

            return _mapper.Map<Session, UpdateSessionViewModel>(session!);
        }

        public bool UpdateSession(int sessionId, UpdateSessionViewModel updateSession)
        {
            try
            {
                var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);

                if (!IsSessionAvilableForUpdating(session!)) return false;

                if (!IsTrainerExsists(updateSession.TrainerId)) return false;

                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;

                _mapper.Map(updateSession, session);
                session!.UpdatedAt = DateTime.Now;

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
                if (!IsSessionAvilableForRemoving(session!)) return false;

                _unitOfWork.GetRepository<Session>().Delete(session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }


        #region Helper Methods
        private bool IsTrainerExsists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(trainerId) is not null;
        }
        private bool IsCategoryExsists(int categoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(categoryId) is not null;
        }
        private bool IsValidDateRange(DateTime startDate, DateTime endDate)
        {
            return startDate < endDate && startDate > DateTime.Now;
        }
        private bool IsSessionAvilableForUpdating(Session session)
        {
            if (session == null) return false;

            if(session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;

        }
        private bool IsSessionAvilableForRemoving(Session session)
        {
            if (session == null) return false;

            if(session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.sessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;

            return true;

        }

        #endregion

    }
}
