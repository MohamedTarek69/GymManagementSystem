using AutoMapper;
using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Way 01

            #region Session Mapping
            //CreateMap<Session, SessionViewModel>()
            //       .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
            //       .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
            //       .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            //CreateMap<CreateSessionViewModel, Session>();

            //CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            #endregion

            #region Plan Mapping
            //CreateMap<Plan, PlanViewModel>().ReverseMap();

            //CreateMap<Plan, UpdatePlanViewModel>().ReverseMap();

            #endregion

            #region Trainer Mapping
            //CreateMap<CreateTrainerViewModel, Trainer>()
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => new Address
            //    {
            //        BuildingNumber = src.BuildingNumber,
            //        Street = src.Street,
            //        City = src.City
            //    }))
            //    .ForMember(dest => dest.Specialties, Options => Options.MapFrom(src => src.Specializations.ToString()))
            //    .ReverseMap();

            //CreateMap<Trainer, TrainerViewModel>()
            //    .ForMember(dest => dest.Specialization, Options => Options.MapFrom(src => src.Specialties.ToString()))
            //    .ReverseMap();

            //CreateMap<Trainer, TrainerToUpdateViewModel>()
            //    .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
            //    .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
            //    .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City))
            //    .ForMember(dest => dest.Specializations, Options => Options.MapFrom(src => src.Specialties.ToString()))
            //    .ReverseMap();

            #endregion

            #region Member Mapping
            //CreateMap<Member, MemberViewModel>()
            //    .ForMember(dest => dest.Gender, Options => Options.MapFrom(src => src.Gender.ToString()))
            //    .ForMember(dest => dest.DateOfBirth, Options => Options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            //    .ForMember(dest => dest.PlanName, Options => Options.Ignore())
            //    .ForMember(destinationMember => destinationMember.MemberShipStartDate, Options => Options.Ignore())
            //    .ForMember(destinationMember => destinationMember.MemberShipEndDate, Options => Options.Ignore())
            //    .ReverseMap();

            //CreateMap<HealthRecord, HealthRecordViewModel>()
            //    .ReverseMap();

            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForMember(dest => dest.Address, Options => Options.MapFrom(src => new Address
            //    {
            //        BuildingNumber = src.BuildingNumber,
            //        Street = src.Street,
            //        City = src.City
            //    }))
            //    .ForMember(dest => dest.HealthRecord, Options => Options.MapFrom(src => src.HealthRecordViewModel))
            //    .ReverseMap();

            //CreateMap<Member, MemberToUpdateViewModel>()
            //    .ForMember(dest => dest.BuildingNumber, Options => Options.MapFrom(src => src.Address.BuildingNumber))
            //    .ForMember(dest => dest.Street, Options => Options.MapFrom(src => src.Address.Street))
            //    .ForMember(dest => dest.City, Options => Options.MapFrom(src => src.Address.City))
            //    .ReverseMap();

            #endregion

            #endregion

            MapTrainer();
            MapSession();
            MapMember();
            MapPlan();
            MapMembership();
            MapBooking();

        }

        private void MapTrainer()
        {
            
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BuildingNumber = src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }))
                .ForMember(dest => dest.Specialties, Options => Options.MapFrom(src => src.Specializations));

            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));
            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Specializations, Options => Options.MapFrom(src => src.Specialties));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .ForMember(dest => dest.Specialties, Options => Options.MapFrom(src => src.Specializations))
            .AfterMap((src, dest) =>
            {
                dest.Address.BuildingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });
        }
        private void MapSession()
        {
            CreateMap<Session, SessionViewModel>()
                   .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                   .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                   .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>().ReverseMap();

            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                  .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                  {
                      BuildingNumber = src.BuildingNumber,
                      City = src.City,
                      Street = src.Street
                  })).ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
            CreateMap<Member, MemberViewModel>()
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
            .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
            .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BuildingNumber = src.BuildingNumber;
                    dest.Address.City = src.City;
                    dest.Address.Street = src.Street;
                    dest.UpdatedAt = DateTime.Now;
                });
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>().ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }
        private void MapMembership()
        {
            CreateMap<MemberShip, MemberShipViewModel>()
                 .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                 .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Plan.Name))
                 .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.CreatedAt.ToString("MMM dd, yyyy")))
                 .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("MMM dd, yyyy")));

            CreateMap<CreateMembershipViewModel, MemberShip>();

            CreateMap<Member, MemberSelectViewModel>();
            CreateMap<Plan, PlanSelectViewModel>();
        }
        private void MapBooking()
        {
            CreateMap<MemberSession, MemberForSessionViewModel>()
                .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.Name))
                .ForMember(dest => dest.BookingDate, opt => opt.MapFrom(src => src.CreatedAt.ToShortDateString()));

            CreateMap<CreateBookingViewModel, MemberSession>()
                .ForMember(dest => dest.IsAttended, opt => opt.Ignore());

        }

    }
}
