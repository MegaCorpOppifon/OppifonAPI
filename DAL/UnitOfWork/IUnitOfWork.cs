using DAL.Repositories;
using System;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAppointmentRepository Appointments { get; }
        ICalendarRepository Calendars { get; }
        ICategoryRepository Categories { get; }
        IDayOffRepository DaysOff { get; }
        IExpertRepository Experts { get; }
        IReviewRepository Reviews { get; }
        ITagRepository Tags { get; }
        IUserRepository Users { get; }
        IWorkDayRepository WorkDays { get; }
        ICalendarAppointmentRepository CalendarAppointments { get; }
        IUserTagRepository UserTags { get; }
        IExpertTagRepository ExpertTags { get; }
        IMainFieldTagRepository MainFieldTags { get; }
        IUserFavoritesRepository UserFavorites { get; }

        int Complete();
    }
}
