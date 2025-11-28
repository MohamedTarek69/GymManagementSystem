using GymManagementBLL.ViewModels.BookingViewModels;
using GymManagementBLL.ViewModels.MemberShipViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;


namespace GymManagementBLL.Services.Interfaces
{
    public interface IBookingService
    {
        IEnumerable<SessionViewModel> GetAllSessionsWithTrainerAndCategory();
        IEnumerable<MemberForSessionViewModel> GetAllMembersForSession(int id);
        bool CreateBooking(CreateBookingViewModel createBooking);
        IEnumerable<MemberSelectViewModel> GetMembersForDropDown(int id);
        bool CancelBooking(MemberAttendOrCancelViewModel model);
        bool MemberAttend(MemberAttendOrCancelViewModel model);
    }
}
