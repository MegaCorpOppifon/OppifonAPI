using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Data;
using DAL.Models.ManyToMany;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DAL.Persistence
{
    public class UserFavoritesRepository : Repository<UserFavorites>, IUserFavoritesRepository
    {
        public UserFavoritesRepository(DbContext context) : base(context)
        {
        }

        public Context OurContext => Context as Context;
        public IQueryable<UserFavorites> GetFavorites(Guid userId)
        {
            return OurContext.UserFavorites.Where(x => x.UserId == userId)
                .Include(x => x.Expert);
        }
    }
}
