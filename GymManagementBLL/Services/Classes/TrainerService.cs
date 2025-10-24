using AutoMapper;
using AutoMapper.Execution;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork=unitOfWork;
            _mapper=mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel trainer)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            if (IsEmailExists(trainer.Email) || IsPhoneExists(trainer.Phone)) return false;

            if (trainer.Specializations == 0) return false;

           var newTrainer = _mapper.Map<CreateTrainerViewModel, Trainer>(trainer);

            TrainerRepo.Add(newTrainer);
            return _unitOfWork.SaveChanges() > 0;
            
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainers = TrainerRepo.GetAll();
            if (trainers == null || !trainers.Any()) return [];

            var trainerViewModels = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainers);

            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(trainerId);

            if (trainer == null) return null;

            var trainerViewModel = _mapper.Map<Trainer, TrainerViewModel>(trainer);
            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(trainerId);

            if (trainer == null) return null;

            var trainerToUpdate = _mapper.Map<Trainer, TrainerToUpdateViewModel>(trainer);
            return trainerToUpdate;
        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                var emailExsists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(X => X.Email == trainerToUpdate.Email && X.Id != trainerId).Any();

                var phoneExsists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(X => X.Phone == trainerToUpdate.Phone && X.Id != trainerId).Any();

                if (emailExsists || phoneExsists) return false;
                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = TrainerRepo.GetById(trainerId);

                if (trainer == null) return false;
                if (trainerToUpdate.Specializations == 0) return false;

                _mapper.Map(trainerToUpdate, trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveTrainer(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var TrainerSessionRepo = _unitOfWork.GetRepository<Session>();

            var trainer = TrainerRepo.GetById(trainerId);
            if (trainer == null) return false;
            
            var HasActiveSessions = TrainerSessionRepo.GetAll(s => s.TrainerId == trainerId);
            if (HasActiveSessions != null && HasActiveSessions.Any()) return false;

            TrainerRepo.Delete(trainer);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region Helper Methods
        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(m => m.Phone == phone).Any();
        }

        #endregion

    }
}
