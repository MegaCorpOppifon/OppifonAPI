using System;
using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public Appointment GetEager(Guid id)
        {
            return OurContext.Appointments.Where(x => x.Id == id)
                .Include(x => x.Participants)
                .Include(x => x.Calendars)
                .SingleOrDefault();
        }
    }
}
