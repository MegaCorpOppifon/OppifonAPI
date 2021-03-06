using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DAL.Persistence
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {}

        public User GetByEmail(string email)
        {
           return OurContext.Users.FirstOrDefault(u => u.UserName == email);
        }

        public User GetEagerByEmail(string email)
        {
            var user = OurContext.Users.FirstOrDefault(u => u.UserName == email);
            return user == null ? null : GetEager(user.Id);
        }

        public User GetEager(Guid id)
        {
            return OurContext.Users
                .Where(x => x.Id == id)
                .Include(x => x.InterestTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.Calendar)
                .ThenInclude(y => y.Appointments)
                .ThenInclude(x => x.Appointment)
                .Include(x => x.Favorites)
                .ThenInclude(x => x.Expert)
                .SingleOrDefault();
        }

        public void RemoveById(Guid id)
        {
            var user = OurContext.Users.SingleOrDefault(x => x.Id == id);
            if (user == null)
                return;

            OurContext.Users.Remove(user);
        }


        public Context OurContext => Context as Context;
      
    }
}
