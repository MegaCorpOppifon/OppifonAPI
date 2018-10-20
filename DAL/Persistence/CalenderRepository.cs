using System;
using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class CalendarRepository : Repository<Calendar>, ICalendarRepository
    {
        public CalendarRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public Calendar GetEagerByUserId(Guid id)
        {
            return OurContext.Calendars
                .Where(x => x.UserId == id)
                .Include(x => x.Appointments)
                .ThenInclude(x => x.Appointment)
                .ThenInclude(x => x.Participants)
                .Include(x => x.DaysOff)
                .Include(x => x.WorkDays)
                .SingleOrDefault();
        }
    }
}
