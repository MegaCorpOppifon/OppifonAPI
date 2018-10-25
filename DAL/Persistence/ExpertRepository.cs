using System;
using System.Collections.Generic;
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
        public Expert GetByEmail(string email)
        {
            return OurContext.Experts.SingleOrDefault(x => x.Email == email);
        }

        public ICollection<Expert> GetExpertsWithTagName(string tagName)
        {
            return OurContext.Experts.Where(x =>
                x.MainFields.Any(y => y.Tag.Name == tagName) ||
                x.ExpertTags.Any(y => y.Tag.Name == tagName))
                .Include(x => x.MainFields)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ExpertTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.ExpertCategory)
                .ToList();
        }

        public Context OurContext => Context as Context;
       
    }
}
