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
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork=unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel trainer)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            if (IsEmailExists(trainer.Email) || IsPhoneExists(trainer.Phone)) return false;

            if (trainer.Specializations == 0) return false;

            var newTrainer = new Trainer()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth,
                Gender = trainer.Gender,
                Address = new Address()
                {
                    BuildingNumber = trainer.BuildingNumber,
                    Street = trainer.Street,
                    City = trainer.City
                },
                Specialties = trainer.Specializations
            };

            TrainerRepo.Add(newTrainer);
            return _unitOfWork.SaveChanges() > 0;
            
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainers = TrainerRepo.GetAll();
            if (trainers == null || !trainers.Any()) return [];

            var trainerViewModels = trainers.Select(trainer => new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialties.ToString(),
            });

            return trainerViewModels;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(trainerId);

            if (trainer == null) return null;

            var trainerViewModel = new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialization = trainer.Specialties.ToString(),
            };
            return trainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var trainer = TrainerRepo.GetById(trainerId);

            if (trainer == null) return null;
            
            var trainerToUpdate = new TrainerToUpdateViewModel()
            {
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specializations = trainer.Specialties
            };
            return trainerToUpdate;
        }

        public bool UpdateTrainerDetails(int trainerId, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                if (IsEmailExists(trainerToUpdate.Email) || IsPhoneExists(trainerToUpdate.Phone)) return false;

                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = TrainerRepo.GetById(trainerId);

                if (trainer == null) return false;
                if (trainerToUpdate.Specializations == 0) return false;

                trainer.Email = trainerToUpdate.Email;
                trainer.Phone = trainerToUpdate.Phone;
                trainer.Address.BuildingNumber = trainerToUpdate.BuildingNumber;
                trainer.Address.Street = trainerToUpdate.Street;
                trainer.Address.City = trainerToUpdate.City;
                trainer.Specialties = trainerToUpdate.Specializations;
                trainer.UpdatedAt = DateTime.Now;

                TrainerRepo.Update(trainer);
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
