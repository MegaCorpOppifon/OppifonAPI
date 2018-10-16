using System.Linq;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class ExpertRepository : Repository<Expert>, IExpertRepository
    {
        public ExpertRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public Expert GetByEmail(string email)
        {
            return OurContext.Experts.SingleOrDefault(x => x.Email == email);
        }
    }
}
