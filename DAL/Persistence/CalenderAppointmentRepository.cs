using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class CalendarAppointmentRepository : Repository<CalendarAppointment>, ICalendarAppointmentRepository
    {
        public CalendarAppointmentRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
