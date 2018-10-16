using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class WorkDayRepository : Repository<WorkDay>, IWorkDayRepository
    {
        public WorkDayRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
