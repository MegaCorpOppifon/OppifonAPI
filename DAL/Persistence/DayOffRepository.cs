using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence 
{
    public class DayOffRepository : Repository<DayOff>, IDayOffRepository
    {
        public DayOffRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
