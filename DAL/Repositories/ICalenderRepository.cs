using System;
using DAL.Models;

namespace DAL.Repositories
{
    public interface ICalendarRepository : IRepository<Calendar>
    {
        Calendar GetEagerByUserId(Guid id);
    }
}
