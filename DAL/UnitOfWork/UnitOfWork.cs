using System;
using System.Linq;
using DAL.Data;
using DAL.Persistence;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork
{
    public class UnitOfWork : IUnityOfWork
    {
        private readonly Context _context;
        
        public UnitOfWork(Context context)
        {
            _context = context;
            Appointments = new AppointmentRepository(_context);
            Calendars = new CalendarRepository(_context);
            Categories = new CategoryRepository(_context);
            DaysOff = new DayOffRepository(_context);
            Experts = new ExpertRepository(_context);
            Reviews = new ReviewRepository(_context);
            Tags = new TagRepository(_context);
            Users = new UserRepository(_context);
            WorkDays = new WorkDayRepository(_context);
            CalendarAppointments = new CalendarAppointmentRepository(_context);
            UserTags = new UserTagRepository(_context);
            ExpertTags = new ExpertTagRepository(_context);
            MainFieldTags = new MainFieldTagRepository(_context);
        }

        public void Dispose()
        {
            try
            {
                _context.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public IAppointmentRepository Appointments { get; }
        public ICalendarRepository Calendars { get; }
        public ICategoryRepository Categories { get; }
        public IDayOffRepository DaysOff { get; }
        public IExpertRepository Experts { get; }
        public IReviewRepository Reviews { get; }
        public ITagRepository Tags { get; }
        public IUserRepository Users { get; }
        public IWorkDayRepository WorkDays { get; }
        public ICalendarAppointmentRepository CalendarAppointments { get; }
        public IUserTagRepository UserTags { get; }
        public IExpertTagRepository ExpertTags { get; }
        public IMainFieldTagRepository MainFieldTags { get; }

        public int Complete()
        {
            var result = 0;
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    saveFailed = true;

                    e.Entries.Single().Reload();
                    result = 1;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            } while (saveFailed);

            return result;
        }
    }
}
