using System;
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

        public Expert GetEagerByEmail(string email)
        {
            var user = OurContext.Users.FirstOrDefault(u => u.UserName == email);
            return user == null ? null : GetEager(user.Id);
        }

        public Expert GetEager(Guid id)
        {
            return OurContext.Experts
                .Where(x => x.Id == id)
                .Include(x => x.InterestTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.Calendar)
                .ThenInclude(y => y.Appointments)
                .SingleOrDefault();
        }

        public Context OurContext => Context as Context;
        public Expert GetByEmail(string email)
        {
            return OurContext.Experts.SingleOrDefault(x => x.Email == email);
        }
    }
}
