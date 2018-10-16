using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class ExpertTagRepository : Repository<ExpertTag>, IExpertTagRepository
    {
        public ExpertTagRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
    }
}
