using System;
using DAL.Models;

namespace DAL.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Appointment GetEager(Guid id);
    }
}
